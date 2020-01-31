using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgSetup;
using CoreWeb.Shell.Views;


namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageServiceAttribute : BaseUserControl, IManageServiceAttributeView
    {
        #region Variables

        #region Private Variables

        private ManageServiceAttributePresenter _presenter = new ManageServiceAttributePresenter();
        private Int32 _tenantid;
        private ServiceAttributeContract currentViewContract = null;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private ManageServiceAttributePresenter Presenter
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

        private ServiceAttributeContract CurrentViewContract
        {
            get
            {
                if (currentViewContract == null)
                    currentViewContract = new ServiceAttributeContract();
                return currentViewContract;
            }
        }

        //private string FilterExpression { get; set; }

        /// <summary>
        /// Gets and sets Logged In User TenantId
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
        /// Gets and Sets list of tenants.
        /// </summary>
        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
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
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.Value;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        public Int32 currentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public Int32? AttributeDataTypeId
        {
            get { return Convert.ToInt32(ViewState["AttributeDataTypeId"]); }
            set { ViewState["AttributeDataTypeId"] = value; }
        }


        #endregion

        #region Public Properties




        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Manage Service Attributes";
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
                ApplyActionLevelPermission(ActionCollection, "Manage Service Attributes");
                BindTenant();
                CaptureQueryString();
            }
            SetDefaultSelectedTenantId();
            base.SetPageTitle("Manage Service Attributes");
        }

        #endregion

        private void CaptureQueryString()
        {

            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("SelectedTenantId"))
                {
                    SelectedTenantId = Convert.ToInt32(args["SelectedTenantId"].ToString());
                    if(SelectedTenantId > Convert.ToInt32(DefaultNumbers.None))
                    {
                        ddlTenant.SelectedValue = SelectedTenantId.ToString();
                    }
                }                
                //if (args.ContainsKey("FilterExpression"))
                //{
                //    FilterExpression = args["FilterExpression"].ToString();
                //}
            }
        }

        #region Grid Events

        protected void grdAttributes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                //if (!IsPostBack 
                //    && !string.IsNullOrEmpty(FilterExpression))
                //{
                //    grdAttributes.MasterTableView.FilterExpression = FilterExpression;                    
                //}
                grdAttributes.DataSource = Presenter.GetServiceAttributes();
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

        protected void grdAttributes_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);

                    WclComboBox cmbDataType = editform.FindControl("cmbDataType") as WclComboBox;

                    cmbDataType.DataSource = Presenter.GetServiceAttributeDataType();
                    cmbDataType.DataBind();

                    cmbDataType.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbDataType_SelectedIndexChanged);

                    BkgSvcAttribute attribute = e.Item.DataItem as BkgSvcAttribute;

                    if (attribute != null)
                    {
                        AttributeDataTypeId = attribute.BSA_DataTypeID;
                        if (!AttributeDataTypeId.IsNullOrEmpty())
                        {
                            cmbDataType.SelectedValue = AttributeDataTypeId.ToString();
                        }
                        System.Web.UI.WebControls.Panel panel = editform.FindControl("pnlAttr") as System.Web.UI.WebControls.Panel;
                        //WclButton btnIsReq = panel.FindControl("btnIsReq") as WclButton;
                        //RequiredFieldValidator rfvReqdMsg = panel.FindControl("rfvReqdMsg") as RequiredFieldValidator;
                        //if (!attribute.BSA_IsRequired.IsNullOrEmpty() && attribute.BSA_IsRequired)
                        //{
                        //    btnIsReq.Checked = true;
                        //    //rfvReqdMsg.Enabled = true;
                        //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "EnableIsRequired", "EnableIsRequired(true);", true);
                        //}
                        ShowHideContentArea(panel, attribute.lkpSvcAttributeDataType.SADT_Name);
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

        protected void grdAttributes_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContract.ServiceAttributeDatatypeID = Convert.ToInt32((e.Item.FindControl("cmbDataType") as WclComboBox).SelectedValue);
                CurrentViewContract.Name = (e.Item.FindControl("txtAttrName") as WclTextBox).Text.Trim();
                CurrentViewContract.AttributeLabel = (e.Item.FindControl("txtAttrLabel") as WclTextBox).Text.Trim();
                CurrentViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();

                String maxChars = (e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text;
                if (maxChars.IsNullOrEmpty())
                    CurrentViewContract.MaximumCharacters = null;
                else
                    CurrentViewContract.MaximumCharacters = Convert.ToInt32(maxChars);

                String minChars = (e.Item.FindControl("ntxtTextMinChars") as WclNumericTextBox).Text;
                if (minChars.IsNullOrEmpty())
                    CurrentViewContract.MinimumCharacters = null;
                else
                    CurrentViewContract.MinimumCharacters = Convert.ToInt32(minChars);

                String maxInt = (e.Item.FindControl("ntxtMaxInt") as WclNumericTextBox).Text;
                if (maxInt.IsNullOrEmpty())
                    CurrentViewContract.MaximumNumericvalue = null;
                else
                    CurrentViewContract.MaximumNumericvalue = Convert.ToInt32(maxInt);

                String minInt = (e.Item.FindControl("ntxtMinInt") as WclNumericTextBox).Text;
                if (minInt.IsNullOrEmpty())
                    CurrentViewContract.MinimumNumericvalue = null;
                else
                    CurrentViewContract.MinimumNumericvalue = Convert.ToInt32(minInt);

                //DateTime maxDate = (e.Item.FindControl("dpMaxDate") as WclDatePicker).SelectedDate.Value;
                if ((e.Item.FindControl("dpMaxDate") as WclDatePicker).SelectedDate.IsNullOrEmpty())
                    CurrentViewContract.MaximumDatevalue = null;
                else
                    CurrentViewContract.MaximumDatevalue = (e.Item.FindControl("dpMaxDate") as WclDatePicker).SelectedDate.Value;

                //DateTime minDate = (e.Item.FindControl("dpMinDate") as WclDatePicker).SelectedDate.Value;
                if ((e.Item.FindControl("dpMinDate") as WclDatePicker).SelectedDate.IsNullOrEmpty())
                    CurrentViewContract.MinimumDatevalue = null;
                else
                    CurrentViewContract.MinimumDatevalue = (e.Item.FindControl("dpMinDate") as WclDatePicker).SelectedDate.Value;

                //String reqValidationMsg = (e.Item.FindControl("txtReqdMsg") as WclTextBox).Text;
                //if (reqValidationMsg.IsNullOrEmpty())
                //    CurrentViewContract.RequiredvalidationMsg = null;
                //else
                //    CurrentViewContract.RequiredvalidationMsg = reqValidationMsg;

                CurrentViewContract.IsActive = (e.Item.FindControl("chkActive") as IsActiveToggle).Checked;
                // CurrentViewContract.IsRequired = (e.Item.FindControl("btnIsReq") as WclButton).Checked;
                CurrentViewContract.IsEditable = true;
                CurrentViewContract.IsSystemPreConfigured = false;
                CurrentViewContract.IsDeleted = false;
                CurrentViewContract.CreatedByID = CurrentUserId;
                CurrentViewContract.CreatedOn = DateTime.Now;
                CurrentViewContract.ModifiedByID = null;
                CurrentViewContract.ModifiedOn = null;
                String options = (e.Item.FindControl("txtOptOptions") as WclTextBox).Text.Trim();

                if (!IsValidOptionFormat(options))
                {
                    base.ShowErrorMessage("Please enter valid options format i.e. Positive=1|Negative=2.");
                    e.Canceled = true;
                    return;
                }

                currentViewContract.lstMasterServiceAttributeOption = GetMasterServiceAttributeOption(options);
                currentViewContract.TenantID = SelectedTenantId;

                if (Presenter.AddServiceAttribute(CurrentViewContract))
                    base.ShowSuccessMessage("Service attribute saved successfully.");
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

        protected void grdAttributes_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                if (AttributeDataTypeId != Convert.ToInt32((e.Item.FindControl("cmbDataType") as WclComboBox).SelectedValue)
                    && !Presenter.IfServiceAttributeCanBeUpdated(Convert.ToInt32(gridEditableItem.GetDataKeyValue("BSA_ID"))))
                {
                    base.ShowErrorInfoMessage(ErrorMessage);
                    e.Canceled = true;
                    return;
                }
                else
                {
                    CurrentViewContract.ServiceAttributeID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BSA_ID"));
                    CurrentViewContract.ServiceAttributeDatatypeID = Convert.ToInt32((e.Item.FindControl("cmbDataType") as WclComboBox).SelectedValue);
                    CurrentViewContract.Name = (e.Item.FindControl("txtAttrName") as WclTextBox).Text.Trim();
                    CurrentViewContract.AttributeLabel = (e.Item.FindControl("txtAttrLabel") as WclTextBox).Text.Trim();
                    CurrentViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                    String maxChars = (e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text;
                    if (maxChars.IsNullOrEmpty())
                        CurrentViewContract.MaximumCharacters = null;
                    else
                        CurrentViewContract.MaximumCharacters = Convert.ToInt32(maxChars);

                    String minChars = (e.Item.FindControl("ntxtTextMinChars") as WclNumericTextBox).Text;
                    if (minChars.IsNullOrEmpty())
                        CurrentViewContract.MinimumCharacters = null;
                    else
                        CurrentViewContract.MinimumCharacters = Convert.ToInt32(minChars);

                    String maxInt = (e.Item.FindControl("ntxtMaxInt") as WclNumericTextBox).Text;
                    if (maxInt.IsNullOrEmpty())
                        CurrentViewContract.MaximumNumericvalue = null;
                    else
                        CurrentViewContract.MaximumNumericvalue = Convert.ToInt32(maxInt);

                    String minInt = (e.Item.FindControl("ntxtMinInt") as WclNumericTextBox).Text;
                    if (minInt.IsNullOrEmpty())
                        CurrentViewContract.MinimumNumericvalue = null;
                    else
                        CurrentViewContract.MinimumNumericvalue = Convert.ToInt32(minInt);

                    //DateTime maxDate = (e.Item.FindControl("dpMaxDate") as WclDatePicker).SelectedDate.Value;
                    if ((e.Item.FindControl("dpMaxDate") as WclDatePicker).SelectedDate.IsNullOrEmpty())
                        CurrentViewContract.MaximumDatevalue = null;
                    else
                        CurrentViewContract.MaximumDatevalue = (e.Item.FindControl("dpMaxDate") as WclDatePicker).SelectedDate.Value;

                    //DateTime minDate = (e.Item.FindControl("dpMinDate") as WclDatePicker).SelectedDate.Value;
                    if ((e.Item.FindControl("dpMinDate") as WclDatePicker).SelectedDate.IsNullOrEmpty())
                        CurrentViewContract.MinimumDatevalue = null;
                    else
                        CurrentViewContract.MinimumDatevalue = (e.Item.FindControl("dpMinDate") as WclDatePicker).SelectedDate.Value;

                    //String reqValidationMsg = (e.Item.FindControl("txtReqdMsg") as WclTextBox).Text;
                    //if (reqValidationMsg.IsNullOrEmpty())
                    //    CurrentViewContract.RequiredvalidationMsg = null;
                    //else
                    //    CurrentViewContract.RequiredvalidationMsg = reqValidationMsg;

                    CurrentViewContract.IsActive = (e.Item.FindControl("chkActive") as IsActiveToggle).Checked;

                    // CurrentViewContract.IsRequired = (e.Item.FindControl("btnIsReq") as WclButton).Checked;
                    CurrentViewContract.IsEditable = true;
                    CurrentViewContract.IsSystemPreConfigured = false;
                    CurrentViewContract.IsDeleted = false;
                    CurrentViewContract.ModifiedByID = CurrentUserId;
                    CurrentViewContract.ModifiedOn = DateTime.Now;
                    String options = (e.Item.FindControl("txtOptOptions") as WclTextBox).Text.Trim();
                    if (!IsValidOptionFormat(options))
                    {
                        base.ShowErrorMessage("Please enter valid options format i.e. Positive=1|Negative=2.");
                        e.Canceled = true;
                        return;
                    }
                    currentViewContract.lstMasterServiceAttributeOption = GetMasterServiceAttributeOption(options);
                    currentViewContract.TenantID = SelectedTenantId;

                    if (Presenter.UpdateServiceAttribute(CurrentViewContract))
                        base.ShowSuccessMessage("Service attribute updated successfully.");
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

        protected void grdAttributes_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 serviceAttributeID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BSA_ID"));
                if (Presenter.DeleteServiceAttribute(serviceAttributeID, CurrentUserId))
                    base.ShowSuccessMessage("Service attribute deleted successfully.");
                else
                    base.ShowErrorInfoMessage(ErrorMessage);
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

        protected void grdAttributes_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    CurrentViewContract.ServiceAttributeID = Convert.ToInt32((e.Item as GridEditableItem).GetDataKeyValue("BSA_ID"));
                }
                else if(e.CommandName == "cascadingDetail")
                {
                    //var filterExpression =  grdAttributes.MasterTableView.FilterExpression;
                    var selectedAttributeId = Convert.ToInt32((e.Item as GridEditableItem).GetDataKeyValue("BSA_ID"));
                    var attributeName = (e.Item as GridEditableItem).GetDataKeyValue("BSA_Name").ToString();
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "AttributeId", selectedAttributeId.ToString()},
                                                                    { "SelectedTenantId", SelectedTenantId.ToString()},
                                                                    //{ "FilterExpression", filterExpression},
                                                                    { "AttributeName", attributeName },
                                                                    { "Child", AppConsts.CASCADING_ATTRIBUTE_DETAILS}
                                                                 };
                    string url = String.Format("~/BkgSetup/Default.aspx?ucid={0}&args={1}", "", queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                       || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdAttributes);
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
        /// Called when data is bound in grid.
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdAttributes_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    dataItem["BSA_Active"].Text = Convert.ToBoolean(dataItem["BSA_Active"].Text) == true ? Convert.ToString("Yes") : Convert.ToString("No");                    
                    HiddenField hdnfIsEditable = e.Item.FindControl("hdnfIsEditable") as HiddenField;

                    var item = dataItem.DataItem as BkgSvcAttribute;

                    if(item.IsNotNull() && item.lkpSvcAttributeDataType.SADT_Code == SvcAttributeDataType.CASCADING.GetStringValue())
                    {
                        var lbtnCascadingDeatil = dataItem.FindControl("cascadingDetail") as LinkButton;
                        lbtnCascadingDeatil.Enabled = true;
                    }                    

                    if (hdnfIsEditable.Value == "False")
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = false;
                        dataItem["EditCommandColumn"].Controls[0].Visible = false;
                    }

                    //dataItem["BSA_Active"].Text = (dataItem.FindControl("IsActive") as Label).Text;
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

        #region DropDown Events

        protected void cmbDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox cmbItemType = sender as WclComboBox;

                System.Web.UI.WebControls.Panel panel = (System.Web.UI.WebControls.Panel)cmbItemType.Parent;
                if (panel != null)
                {
                    ShowHideContentArea(panel, e.Text, true);
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
        /// Binds the attributes as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                grdAttributes.CurrentPageIndex = 0;
                grdAttributes.MasterTableView.SortExpressions.Clear();
                grdAttributes.MasterTableView.FilterExpression = null;

                foreach (GridColumn column in grdAttributes.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                grdAttributes.EditIndexes.Clear();
                grdAttributes.MasterTableView.IsItemInserted = false;
                grdAttributes.Rebind();
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

        private void ShowHideContentArea(System.Web.UI.WebControls.Panel panel, String serviceAttributeDatatype, bool clearData = false)
        {

            System.Web.UI.HtmlControls.HtmlGenericControl divOption = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divOption");
            if (divOption != null)
            {
                if (clearData)
                {
                    WclTextBox txtOptOptions = panel.FindControl("txtOptOptions") as WclTextBox;
                    txtOptOptions.Text = String.Empty;

                }
                divOption.Visible = false;
            }


            System.Web.UI.HtmlControls.HtmlGenericControl divCharacters = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divCharacters");
            if (divCharacters != null)
            {
                if (clearData)
                {
                    WclNumericTextBox ntxtTextMaxChars = panel.FindControl("ntxtTextMaxChars") as WclNumericTextBox;
                    ntxtTextMaxChars.Text = String.Empty;
                    WclNumericTextBox ntxtTextMinChars = panel.FindControl("ntxtTextMinChars") as WclNumericTextBox;
                    ntxtTextMinChars.Text = String.Empty;
                }
                divCharacters.Visible = false;
            }

            System.Web.UI.HtmlControls.HtmlGenericControl divNumeric = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divNumeric");
            if (divNumeric != null)
            {
                if (clearData)
                {
                    WclNumericTextBox ntxtMinInt = panel.FindControl("ntxtMinInt") as WclNumericTextBox;
                    ntxtMinInt.Text = String.Empty;
                    WclNumericTextBox ntxtMaxInt = panel.FindControl("ntxtMaxInt") as WclNumericTextBox;
                    ntxtMaxInt.Text = String.Empty;
                }
                divNumeric.Visible = false;
            }

            System.Web.UI.HtmlControls.HtmlGenericControl divDate = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divDate");
            if (divDate != null)
            {
                if (clearData)
                {
                    WclDatePicker minDate = panel.FindControl("dpMinDate") as WclDatePicker;
                    minDate.SelectedDate = null;
                    WclDatePicker maxDate = panel.FindControl("dpMaxDate") as WclDatePicker;
                    maxDate.SelectedDate = null;
                }
                divDate.Visible = false;
            }

            //if ((panel.FindControl("btnIsReq") as WclButton).Checked)
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "EnableIsRequired", "EnableIsRequired(true);", true);
            //}
            //else
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "EnableIsRequired", "EnableIsRequired(false);", true);
            //}

            if (divOption != null && serviceAttributeDatatype.Equals("Option"))
                divOption.Visible = true;

            if (divCharacters != null && serviceAttributeDatatype.Equals("Text"))
                divCharacters.Visible = true;

            if (divNumeric != null && serviceAttributeDatatype.Equals("Numeric"))
                divNumeric.Visible = true;

            if (divDate != null && serviceAttributeDatatype.Equals("Date"))
                divDate.Visible = true;

        }

        private Boolean IsValidOptionFormat(String options)
        {
            if (!String.IsNullOrEmpty(options))
            {
                //UAT-3486
                //string[] arrayOfOptions = options.Split(',');
                string[] arrayOfOptions = options.Split('|');
                if (arrayOfOptions.Length > 0)
                {
                    for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                    {
                        string[] option = arrayOfOptions[counter].Split('=');
                        if (!option.Length.Equals(2) || String.IsNullOrEmpty(option[1]))
                            return false;

                    }
                }
                else
                    return false;
            }
            return true;
        }

        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> GetMasterServiceAttributeOption(String options)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> lstServiceAttributeOption = null;
            if (String.IsNullOrEmpty(options))
                return lstServiceAttributeOption;
            
            //UAT-3486
            //string[] arrayOfOptions = options.Split(',');
            string[] arrayOfOptions = options.Split('|');

            if (arrayOfOptions.Length > 0)
            {
                for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                {
                    string[] option = arrayOfOptions[counter].Split('=');
                    if (option.Length.Equals(2))
                    {
                        if (lstServiceAttributeOption == null)
                            lstServiceAttributeOption = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption>();

                        lstServiceAttributeOption.Add(new Entity.BkgSvcAttributeOption()
                        {
                            EBSAO_OptionText = option[0],
                            EBSAO_OptionValue = option[1],
                            EBSAO_CreatedByID = CurrentUserId,
                            EBSAO_CreatedOn = DateTime.Now,
                            EBSAO_IsActive = true,
                            EBSAO_IsDeleted = false

                        });
                    }
                }
            }
            return lstServiceAttributeOption;
        }

        private void BindTenant()
        {
            if (IsAdminLoggedIn == true)
            {
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
                ddlTenant.SelectedValue = Convert.ToString(TenantId);
                SelectedTenantId = TenantId;
            }
        }

        #endregion

        #region Apply Permissions

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Add",
                    CustomActionLabel = "Add New",
                    ScreenName = "Manage Service Attributes"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Manage Service Attributes"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Manage Service Attributes"
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
                            if (x.FeatureAction.CustomActionId == "Add")
                            {
                                grdAttributes.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                            }
                            else if (x.FeatureAction.CustomActionId == "Edit")
                            {
                                grdAttributes.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                            }
                            else if (x.FeatureAction.CustomActionId == "Delete")
                            {
                                grdAttributes.MasterTableView.GetColumn("DeleteColumn").Display = false;
                            }
                            break;
                        }
                }

            }
                );
        }

        #endregion

        //protected void grdAttributes_PreRender(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(FilterExpression))
        //    {
        //        grdAttributes.MasterTableView.FilterExpression = FilterExpression;
        //        grdAttributes.Rebind();
        //    }
        //}
    }
}