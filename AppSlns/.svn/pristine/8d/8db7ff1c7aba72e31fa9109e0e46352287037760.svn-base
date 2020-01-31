#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

#endregion

#region Project Specific
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Data;
using INTSOF.UI.Contract.BkgSetup;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageFeeRecord : BaseUserControl, IManageFeeRecordView
    {
        #region Variables

        #region Private Variables

        private ManageFeeRecordPresenter _presenter = new ManageFeeRecordPresenter();
        private Int32 _tenantid;
        private String _viewType;
        private Boolean? _isAdminLoggedIn = null;

        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties

        public ManageFeeRecordPresenter Presenter
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
        /// Gets and sets Logged In User TenantId
        /// </summary>
        Int32 IManageFeeRecordView.TenantId
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
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> IManageFeeRecordView.ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 IManageFeeRecordView.SelectedTenantId
        {
            get
            {
                if (CurrentViewContext.IsAdminLoggedIn)
                {
                    if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                        return Convert.ToInt32(ddlTenant.SelectedValue);
                    return AppConsts.NONE;
                }
                else
                    return CurrentViewContext.TenantId;
            }
            set
            {
                ddlTenant.SelectedValue = Convert.ToString(value);
            }
        }

        Boolean IManageFeeRecordView.IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.Value;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        Int32 IManageFeeRecordView.currentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        String IManageFeeRecordView.ErrorMessage
        {
            get;
            set;
        }

        String IManageFeeRecordView.FieldValue
        {
            get;
            set;
        }
        String IManageFeeRecordView.SuccessMessage
        {
            get;
            set;
        }
        String IManageFeeRecordView.InfoMessage
        {
            get;
            set;
        }


        /// <summary>
        /// Gets and Sets list of Package Service Item Fee
        /// </summary>
        List<PackageServiceItemFee> IManageFeeRecordView.ListPackageServiceItemFee
        {
            set;
            get;
        }

        Decimal IManageFeeRecordView.Amount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<ServiceItemFeeRecord> IManageFeeRecordView.ListServiceItemFeeRecord
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and set List of All States 
        /// </summary>
        List<Entity.State> IManageFeeRecordView.ListAllState
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and set List of All Countries 
        /// </summary>
        List<Country> IManageFeeRecordView.lstCountries
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and set List of County corresponding to stateid. 
        /// </summary>
        List<Entity.County> IManageFeeRecordView.ListCountyByStateId
        {
            set;
            get;
        }

        Int32 IManageFeeRecordView.SelectedFeeItemId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedFeeItemId"]);
            }
            set
            {
                ViewState["SelectedFeeItemId"] = value;
            }
        }

        public String FeeTypeCode
        {
            get
            {
                return (String)(ViewState["FeeTypeCode"]);
            }
            set
            {
                ViewState["FeeTypeCode"] = value;
            }
        }

        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<ServiceFeeItemRecordContract> IManageFeeRecordView.ListServiceItemFeeRecordContract
        {
            set;
            get;
        }
        List<lkpCabsMailingOption> IManageFeeRecordView.lstMailingOption
        {
            get;
            set;
        }

        List<lkpAdditionalServiceFeeType> IManageFeeRecordView.lstAdditionalServiceFeeOption
        {
            get;
            set;
        }
        #region Current View Context
        private IManageFeeRecordView CurrentViewContext
        {
            get { return this; }
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
                base.Title = "Manage Global Fee Record";
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
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    ApplyActionLevelPermission(ActionCollection, "Manage Global Fee Record");
                    //BindTenant();
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (!Request.QueryString["args"].IsNull())
                    {
                        if (args.ContainsKey("PSIF_ID"))
                        {
                            CurrentViewContext.SelectedFeeItemId = Convert.ToInt32(args["PSIF_ID"]);
                        }
                        if (args.ContainsKey("SIFT_Code"))
                        {
                            FeeTypeCode = Convert.ToString(args["SIFT_Code"]);
                        }
                        if (args.ContainsKey("FeeItemName"))
                        {
                            hdnFeeItemName.Value = Convert.ToString(args["FeeItemName"]);
                        }
                    }


                    //UAT:-3011: Add State column to individual county fee grid

                    if (FeeTypeCode == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
                    {
                        grdFeeRecord.Columns.FindByUniqueName("State").Visible = true;
                        grdFeeRecord.Columns.FindByUniqueName("FieldValue").HeaderText = "County";
                    }

                }
                lblFeeItem.Text = hdnFeeItemName.Value.HtmlEncode();
                ShowHideGrid();
                base.SetPageTitle("Manage Global Fee Record");
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Grid Events

        protected void grdFeeRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetServiceItemFeeRecordListContract();
                grdFeeRecord.DataSource = CurrentViewContext.ListServiceItemFeeRecordContract;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdFeeRecord_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    //WclComboBox cmbFeeItem = editform.FindControl("cmbFeeItem") as WclComboBox;
                    WclComboBox cmbState = editform.FindControl("cmbState") as WclComboBox;
                    WclComboBox cmbCounty = editform.FindControl("cmbCounty") as WclComboBox;
                    HtmlGenericControl divCounty = editform.FindControl("divCounty") as HtmlGenericControl;
                    HtmlGenericControl divCountry = editform.FindControl("divCountry") as HtmlGenericControl;
                    HtmlGenericControl divStateCounty = editform.FindControl("divStateCounty") as HtmlGenericControl;
                    HtmlGenericControl divShipping = editform.FindControl("divShipping") as HtmlGenericControl;
                    HtmlGenericControl divAdditionalService = editform.FindControl("divAdditionalService") as HtmlGenericControl;                    
                    WclNumericTextBox ntxtAmount = editform.FindControl("ntxtAmount") as WclNumericTextBox;
                    //Presenter.GetPackageSvcItemFeeList();
                    //cmbFeeItem.DataSource = CurrentViewContext.ListPackageServiceItemFee;
                    //cmbFeeItem.DataBind();
                    // cmbFeeItem.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbFeeItem_SelectedIndexChanged);
                    BindStateComboBox(cmbState, divStateCounty, divCounty);
                    if (!(e.Item.DataItem is GridInsertionObject))
                    {
                        //ServiceItemFeeRecord svcItemFeeRecord = e.Item.DataItem as ServiceItemFeeRecord;
                        ServiceFeeItemRecordContract svcItemFeeRecord = e.Item.DataItem as ServiceFeeItemRecordContract;
                        if (svcItemFeeRecord != null)
                        {
                            //CurrentViewContext.SelectedFeeItemId = svcItemFeeRecord.SIFR_FeeeItemId.Value;
                            //FeeTypeCode = svcItemFeeRecord.PackageServiceItemFee.lkpServiceItemFeeType.SIFT_Code;
                            FeeTypeCode = svcItemFeeRecord.FeeItemTypeCode;
                            CurrentViewContext.SelectedFeeItemId = svcItemFeeRecord.SIFR_FeeItemID;
                            if (!CurrentViewContext.SelectedFeeItemId.IsNullOrEmpty())
                            {
                                //cmbFeeItem.SelectedValue = CurrentViewContext.SelectedFeeItemId.ToString();
                                if (FeeTypeCode == ServiceItemFeeType.STATE_COURT_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.ALL_COUNTY_COURT_FEE.GetStringValue()
                                    || FeeTypeCode == ServiceItemFeeType.MVR_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
                                {
                                    divStateCounty.Visible = true;
                                    divCountry.Visible = false;
                                    Presenter.GetAllState();
                                    BindCombo(cmbState, CurrentViewContext.ListAllState);
                                    if (FeeTypeCode == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
                                    {
                                        divCounty.Visible = true;
                                        // Entity.County county= Presenter.GetCountyByCountyId(Convert.ToInt32(svcItemFeeRecord.SIFR_FieldValue));
                                        Entity.County county = Presenter.GetCountyByCountyId(Convert.ToInt32(svcItemFeeRecord.FieldID));
                                        Presenter.GetCountyByStateId(county.StateID);
                                        BindCombo(cmbCounty, CurrentViewContext.ListCountyByStateId);
                                        cmbState.SelectedValue = county.StateID.ToString();
                                        //cmbCounty.SelectedValue = svcItemFeeRecord.SIFR_FieldValue;
                                        cmbCounty.SelectedValue = svcItemFeeRecord.FieldID;
                                    }
                                    else if (FeeTypeCode == ServiceItemFeeType.ALL_COUNTY_COURT_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.STATE_COURT_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.MVR_FEE.GetStringValue())
                                    {
                                        divCounty.Visible = false;
                                        //cmbState.SelectedValue = svcItemFeeRecord.SIFR_FieldValue;
                                        cmbState.SelectedValue = svcItemFeeRecord.FieldID;
                                        //RadComboBoxItem item = cmbState.Items.FindItemByText("SIFR_FieldValue");
                                        //item
                                    }
                                }
                                else if (FeeTypeCode == ServiceItemFeeType.COUNTRY_FEE.GetStringValue())
                                {
                                    WclComboBox cmbCountries = editform.FindControl("cmbCountry") as WclComboBox;
                                    BindCountries(cmbCountries);
                                    cmbCountries.SelectedValue = svcItemFeeRecord.FieldID;
                                    divCountry.Visible = true;
                                }
                                else if (FeeTypeCode == ServiceItemFeeType.SHIPPING_FEE.GetStringValue())
                                {
                                    WclComboBox cmbMailingOption = editform.FindControl("cmbMailingOption") as WclComboBox;                                                                       
                                    BindMailingOption(cmbMailingOption);
                                    cmbMailingOption.SelectedValue = svcItemFeeRecord.FieldID;
                                    divShipping.Visible = true;
                                    ntxtAmount.MinValue = 0;
                                }
                                else if (FeeTypeCode == ServiceItemFeeType.ADDITIONAL_SERVICE_FEE.GetStringValue())
                                {
                                    WclComboBox cmbAdditionalServiceFeeOption = editform.FindControl("cmbAdditionalService") as WclComboBox;
                                    BindAdditionalServiceFeeOption(cmbAdditionalServiceFeeOption);
                                    cmbAdditionalServiceFeeOption.SelectedValue = svcItemFeeRecord.FieldID;
                                    divAdditionalService.Visible = true;                                    
                                }
                            }
                        }
                    }
                    else
                    {
                        if (FeeTypeCode == ServiceItemFeeType.COUNTRY_FEE.GetStringValue())
                        {
                            WclComboBox cmbCountries = editform.FindControl("cmbCountry") as WclComboBox;
                            BindCountries(cmbCountries);
                            divCountry.Visible = true;
                        }
                        else if (FeeTypeCode == ServiceItemFeeType.SHIPPING_FEE.GetStringValue())
                        {
                            WclComboBox cmbMailingOption = editform.FindControl("cmbMailingOption") as WclComboBox;
                            BindMailingOption(cmbMailingOption);
                            divShipping.Visible = true;
                            ntxtAmount.MinValue = 0;
                        }
                        else if (FeeTypeCode == ServiceItemFeeType.ADDITIONAL_SERVICE_FEE.GetStringValue())
                        {
                            WclComboBox cmbAdditionalServiceFeeOption = editform.FindControl("cmbAdditionalService") as WclComboBox;
                            BindAdditionalServiceFeeOption(cmbAdditionalServiceFeeOption);
                            divAdditionalService.Visible = true;                          
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdFeeRecord_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                //CurrentViewContext.SelectedFeeItemId = Convert.ToInt32((e.Item.FindControl("cmbFeeItem") as WclComboBox).SelectedValue);
                if (FeeTypeCode == ServiceItemFeeType.ALL_COUNTY_COURT_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.STATE_COURT_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.MVR_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbState") as WclComboBox).SelectedValue;
                }
                else if (FeeTypeCode == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbCounty") as WclComboBox).SelectedValue;
                }
                else if (FeeTypeCode == ServiceItemFeeType.COUNTRY_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbCountry") as WclComboBox).SelectedValue;
                }
                else if (FeeTypeCode == ServiceItemFeeType.SHIPPING_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbMailingOption") as WclComboBox).SelectedValue;
                }
                else if (FeeTypeCode == ServiceItemFeeType.ADDITIONAL_SERVICE_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbAdditionalService") as WclComboBox).SelectedValue;
                }
                //else
                //{
                //    CurrentViewContext.FieldValue = "$$$Not Applicable";
                //}
                CurrentViewContext.Amount = Convert.ToDecimal((e.Item.FindControl("ntxtAmount") as WclNumericTextBox).Text.Trim());
                Presenter.SaveServiceItemFeeRecordData();
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdFeeRecord_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 svcItemFeeRecordId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("SIFR_ID"));
                // CurrentViewContext.SelectedFeeItemId = Convert.ToInt32((e.Item.FindControl("cmbFeeItem") as WclComboBox).SelectedValue);
                if (FeeTypeCode == ServiceItemFeeType.ALL_COUNTY_COURT_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.STATE_COURT_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.MVR_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbState") as WclComboBox).SelectedValue;
                }
                else if (FeeTypeCode == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbCounty") as WclComboBox).SelectedValue;
                }
                else if (FeeTypeCode == ServiceItemFeeType.COUNTRY_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbCountry") as WclComboBox).SelectedValue;
                }
                else if (FeeTypeCode == ServiceItemFeeType.SHIPPING_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbMailingOption") as WclComboBox).SelectedValue;
                }
                else if (FeeTypeCode == ServiceItemFeeType.ADDITIONAL_SERVICE_FEE.GetStringValue())
                {
                    CurrentViewContext.FieldValue = (e.Item.FindControl("cmbAdditionalService") as WclComboBox).SelectedValue;
                }
                //else
                //{
                //    CurrentViewContext.FieldValue = "$$$Not Applicable";
                //}
                CurrentViewContext.Amount = Convert.ToDecimal((e.Item.FindControl("ntxtAmount") as WclNumericTextBox).Text.Trim());
                Presenter.UpdateServiceItemFeeRecordData(svcItemFeeRecordId);
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdFeeRecord_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 svcItemFeeRecordId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("SIFR_ID"));
                Presenter.DeleteServiceItemFeeRecordData(svcItemFeeRecordId);
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region DropDown Events

        /// <summary>
        /// Binds the attributes as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    grdFeeRecord.Rebind();
                    grdFeeRecord.Visible = true;
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

        //protected void cmbFeeItem_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    try
        //    {
        //        WclComboBox cmbFeeItem = sender as WclComboBox;
        //        if (Convert.ToInt32(cmbFeeItem.SelectedValue) != -1)
        //        {
        //            CurrentViewContext.SelectedFeeItemId = Convert.ToInt32(cmbFeeItem.SelectedValue);
        //            FeeTypeCode = cmbFeeItem.SelectedItem.Attributes["FeeTypeCode"];
        //            HtmlGenericControl divStateCounty = cmbFeeItem.Parent.NamingContainer.FindControl("divStateCounty") as HtmlGenericControl;
        //            HtmlGenericControl divCounty = cmbFeeItem.Parent.NamingContainer.FindControl("divCounty") as HtmlGenericControl;
        //            if (FeeTypeCode == ServiceItemFeeType.STATE_COURT_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.ALL_COUNTY_COURT_FEE.GetStringValue()
        //                            || FeeTypeCode == ServiceItemFeeType.MVR_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
        //            {
        //                divStateCounty.Visible = true;
        //                WclComboBox cmbState = cmbFeeItem.Parent.NamingContainer.FindControl("cmbState") as WclComboBox;
        //                if (cmbState.IsNotNull())
        //                {
        //                    Presenter.GetAllState();
        //                    if (CurrentViewContext.ListAllState.IsNotNull())
        //                    {
        //                        BindCombo(cmbState, CurrentViewContext.ListAllState);
        //                    }
        //                }
        //                divCounty.Visible = false;
        //            }
        //            else
        //            {
        //                divStateCounty.Visible = false;
        //            }
        //        }
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

        protected void cmbFeeItem_ItemDataBound(object o, RadComboBoxItemEventArgs e)
        {
            PackageServiceItemFee pkgSvcItemFee = (PackageServiceItemFee)e.Item.DataItem;
            if (pkgSvcItemFee.PSIF_ID != -1)
                e.Item.Attributes["FeeTypeCode"] = pkgSvcItemFee.lkpServiceItemFeeType.SIFT_Code.ToString();
        }

        protected void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                WclComboBox cmbState = sender as WclComboBox;
                HtmlGenericControl divCounty = cmbState.Parent.NamingContainer.FindControl("divCounty") as HtmlGenericControl;
                if (Convert.ToInt32(cmbState.SelectedValue) > 0)
                {
                    Int32 selectedStateId = Convert.ToInt32(cmbState.SelectedValue);
                    WclComboBox cmbCounty = cmbState.Parent.NamingContainer.FindControl("cmbCounty") as WclComboBox;

                    if (FeeTypeCode == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
                    {
                        divCounty.Visible = true;
                        if (cmbCounty.IsNotNull())
                        {
                            Presenter.GetCountyByStateId(selectedStateId);
                            if (CurrentViewContext.ListCountyByStateId.IsNotNull())
                            {
                                BindCombo(cmbCounty, CurrentViewContext.ListCountyByStateId);
                            }
                        }
                    }
                    else
                    {
                        divCounty.Visible = false;
                    }
                }
                else
                {
                    divCounty.Visible = false;
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
        protected void CmdBarCancel_Click(Object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.ManageFeeItem}
                                                                 
                                                                   
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }
        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Method to bind Tenant drop down if admin loggedIn or hide for client admin.
        /// </summary>
        private void BindTenant()
        {
            if (CurrentViewContext.IsAdminLoggedIn == true)
            {
                Presenter.GetTenants();
                ddlTenant.DataSource = CurrentViewContext.ListTenants;
                ddlTenant.DataBind();
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }

        /// <summary>
        /// Set the selected 
        /// </summary>
        private void ShowHideGrid()
        {
            if (!CurrentViewContext.IsAdminLoggedIn)
            {
                divGrdFeeRecord.Visible = false;
                divMainSection.Visible = false;
                base.ShowErrorInfoMessage("You don't have access to this page.Please contact administrator.");
            }
            else
            {
                divGrdFeeRecord.Visible = true;
                divMainSection.Visible = true;
            }
        }

        private void ShowHideComboBox()
        {

        }

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            //if (dataSource==String.Empty)
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
        }

        private void BindStateComboBox(WclComboBox cmbState, HtmlGenericControl divStateCounty, HtmlGenericControl divCounty)
        {
            if (FeeTypeCode == ServiceItemFeeType.STATE_COURT_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.ALL_COUNTY_COURT_FEE.GetStringValue()
                            || FeeTypeCode == ServiceItemFeeType.MVR_FEE.GetStringValue() || FeeTypeCode == ServiceItemFeeType.COUNTY_COURT_FEE.GetStringValue())
            {
                divStateCounty.Visible = true;
                //WclComboBox cmbState = cmbFeeItem.Parent.NamingContainer.FindControl("cmbState") as WclComboBox;
                if (cmbState.IsNotNull())
                {
                    Presenter.GetAllState();
                    if (CurrentViewContext.ListAllState.IsNotNull())
                    {
                        BindCombo(cmbState, CurrentViewContext.ListAllState);
                    }
                }
                divCounty.Visible = false;
            }
            else
            {
                divStateCounty.Visible = false;
            }
        }

        /// <summary>
        /// Bind the List of Countries for the International Criminal Search global Fee
        /// </summary>
        /// <param name="cmbCountries"></param>
        private void BindCountries(WclComboBox cmbCountries)
        {
            Presenter.GetCountries();

            CurrentViewContext.lstCountries.Insert(AppConsts.NONE, new Country
            {
                FullName = "--Select--",
                CountryID = AppConsts.NONE
            });

            cmbCountries.DataSource = CurrentViewContext.lstCountries;
            cmbCountries.DataTextField = "FullName";
            cmbCountries.DataValueField = "CountryID";
            cmbCountries.DataBind();
        }

        private void BindMailingOption(WclComboBox cmbMailingOption)
        {
            Presenter.GetMailingOption();
            CurrentViewContext.lstMailingOption.Insert(AppConsts.NONE, new lkpCabsMailingOption
            {
                CMO_Name = "--Select--",
                CMO_ID = AppConsts.NONE
            });
            cmbMailingOption.DataSource = CurrentViewContext.lstMailingOption;
            cmbMailingOption.DataTextField = "CMO_Name";
            cmbMailingOption.DataValueField = "CMO_ID";
            cmbMailingOption.DataBind();
        }

        private void BindAdditionalServiceFeeOption(WclComboBox cmbAdditionalServiceFeeOption)
        {
            Presenter.GetAdditionalServiceFeeOption();
            CurrentViewContext.lstAdditionalServiceFeeOption.Insert(AppConsts.NONE, new lkpAdditionalServiceFeeType
            {
                LASF_Name = "--Select--",
                LASF_ID = AppConsts.NONE
            });
            cmbAdditionalServiceFeeOption.DataSource = CurrentViewContext.lstAdditionalServiceFeeOption;
            cmbAdditionalServiceFeeOption.DataTextField = "LASF_Name";
            cmbAdditionalServiceFeeOption.DataValueField = "LASF_ID";
            cmbAdditionalServiceFeeOption.DataBind();
        }

        #endregion

        #region Apply Permissions

        /// <summary>
        /// Override base ActionCollection property to set the action collections.
        /// </summary>
        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Add",
                    CustomActionLabel = "Add New",
                    ScreenName = "Manage Global Fee Record"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Manage Global Fee Record"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Manage Global Fee Record"
                });
                return actionCollection;
            }
        }

        /// <summary>
        /// Override ApplyActionLevelPermission to apply the action permissions.
        /// </summary>
        /// <param name="ctrlCollection"></param>
        /// <param name="screenName"></param>
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
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
                            if (x.FeatureAction.CustomActionId == "Add")
                            {
                                grdFeeRecord.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                            }
                            else if (x.FeatureAction.CustomActionId == "Edit")
                            {
                                grdFeeRecord.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                            }
                            else if (x.FeatureAction.CustomActionId == "Delete")
                            {
                                grdFeeRecord.MasterTableView.GetColumn("DeleteColumn").Display = false;
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