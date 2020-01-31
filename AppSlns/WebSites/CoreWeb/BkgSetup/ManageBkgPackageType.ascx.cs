using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.BkgSetup.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup
{
    public partial class ManageBkgPackageType : BaseUserControl, IManagePackageTypeView
    {
        #region Variables

        #region Private variables
        private ManagePackageTypePresenter _presenter = new ManagePackageTypePresenter();
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _tenantid;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties
        public ManagePackageTypePresenter Presenter
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

        public IManagePackageTypeView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        public List<Tenant> ListTenants
        {
            get
            {
                if (!ViewState["ListTenants"].IsNull())
                {
                    return ViewState["ListTenants"] as List<Tenant>;
                }

                return new List<Tenant>();
            }
            set
            {
                ViewState["ListTenants"] = value;
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
        public String PackageTypeName
        {
            get;
            set;
        }
        public String PackageTypeCode
        {
            get;
            set;
        }
        public String PackageTypeColorCode
        {
            //get;
            //set;
            get
            {
                if (!ViewState["PackageTypeColorCode"].IsNull())
                {
                    return Convert.ToString(ViewState["PackageTypeColorCode"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["PackageTypeColorCode"] = value;
            }
        }


        //public Int32 BkgPackageTypeTenantId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenant.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenant.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlTenant.SelectedValue = Convert.ToString(value);
                }
                else
                {
                    ddlTenant.SelectedIndex = value;
                }
            }
        }


        public Int32 SelectTenantIdForAddForm
        {
            get;
            set;
        }
        public List<BkgPackageType> lstBkgPackageType
        {
            get;
            set;
        }

        public String LastCode
        {
            get;
            set;
        }
        public Int32 BkgPackageTypeId
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get;
            set;
        }
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdBkgPackageType.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdBkgPackageType.MasterTableView.CurrentPageIndex > 0)
                {
                    grdBkgPackageType.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        public Int32 PageSize
        {
            get
            {
                return grdBkgPackageType.MasterTableView.PageSize;
            }
            set
            {
                grdBkgPackageType.MasterTableView.PageSize = value;
            }
        }

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
                grdBkgPackageType.VirtualItemCount = value;
                grdBkgPackageType.MasterTableView.VirtualItemCount = value;
            }
        }

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
                CurrentViewContext.VirtualRecordCount = value.VirtualPageCount;
                CurrentViewContext.PageSize = value.PageSize;
                CurrentViewContext.CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        string IManagePackageTypeView.PackageTypeName
        {
            get
            {
                return ViewState["PackageTypeName"].IsNullOrEmpty() ? "" : (String)(ViewState["PackageTypeName"]);
            }
            set { ViewState["PackageTypeName"] = value; }
        }

        string IManagePackageTypeView.PackageTypeCode
        {
            get
            {
                return ViewState["PackageTypeCode"].IsNullOrEmpty() ? "" : (String)(ViewState["PackageTypeCode"]);
            }
            set { ViewState["PackageTypeCode"] = value; }

        }
        int IManagePackageTypeView.CurrentLoggedInUserId
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"]);
                }
                else
                {
                    return SysXWebSiteUtils.SessionService.OrganizationUserId;
                }
            }
        }
        #endregion

        #region Private Properties

        #endregion

        #endregion

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Package Type";
                base.SetPageTitle("Manage Package Type");
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
                    grdBkgPackageType.Visible = false;
                    Presenter.OnViewInitialized();
                    BindTenant();
                    fsucCmdBarButton.SaveButton.ValidationGroup = "grpFormSearch";
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


        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {            
            ddlTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }
        #region Private Methods

        private void BindTenant()
        {
            if (IsAdminLoggedIn == true)
            {
                Presenter.GetTenants();
                ddlTenant.DataSource = ListTenants;
                ddlTenant.DataBind();
            }
        }
        #endregion

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetGridFilters();
            ResetControls();
            if (!ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
            }
          
        }

        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                ddlTenant.SelectedValue = AppConsts.ZERO;
                ResetControls();
                ResetGridFilters();
            
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

        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            grdBkgPackageType.Visible = true;
            ViewState["PackageTypeName"] = txtPackageTypeName.Text;
            ViewState["PackageTypeCode"] = txtPackageTypeCode.Text;           
            ResetGridFilters();
        }

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
        }

        protected void grdBkgPackageType_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;                  
                    CurrentViewContext.BkgPackageTypeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BPT_Id"));
                    Presenter.DeletePackageType();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        CurrentViewContext.BkgPackageTypeId = 0;
                        base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                        grdBkgPackageType.Rebind();
                    }
                    else
                    {
                        e.Canceled = false;                    
                        CurrentViewContext.BkgPackageTypeId = 0;
                        base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    }
                }
                else if (e.CommandName == RadGrid.UpdateCommandName)
                {
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    RadColorPicker radColorPicker = e.Item.FindControl("crpPieChartColor") as RadColorPicker;
                    CurrentViewContext.SelectTenantIdForAddForm = Convert.ToInt32((e.Item.FindControl("ddlTenantName") as WclComboBox).SelectedValue.Trim());
                    CurrentViewContext.PackageTypeName = (e.Item.FindControl("txtPkgTypeName") as WclTextBox).Text.Trim();
                    CurrentViewContext.PackageTypeCode = (e.Item.FindControl("txtPkgTypeCode") as WclTextBox).Text.Trim();
                    CurrentViewContext.BkgPackageTypeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BPT_Id"));
                    CurrentViewContext.PackageTypeColorCode = ColorTranslator.ToHtml(radColorPicker.SelectedColor);
                    Presenter.UpdatePackageType();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;                       
                        CurrentViewContext.BkgPackageTypeId = 0;
                        base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                    else
                    {
                        e.Canceled = false;
                        CurrentViewContext.BkgPackageTypeId = 0;
                        CurrentViewContext.PackageTypeColorCode = String.Empty;
                        base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    }
                }
                else if (e.CommandName == RadGrid.PerformInsertCommandName)
                {
                    CurrentViewContext.SelectTenantIdForAddForm = Convert.ToInt32((e.Item.FindControl("ddlTenantName") as WclComboBox).SelectedValue.Trim());
                    CurrentViewContext.PackageTypeName = (e.Item.FindControl("txtPkgTypeName") as WclTextBox).Text.Trim();
                    CurrentViewContext.PackageTypeCode = (e.Item.FindControl("txtPkgTypeCode") as WclTextBox).Text.Trim();
                    Presenter.SavePackageType();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;                                             
                        CurrentViewContext.BkgPackageTypeId = 0;
                        base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                    else
                    {
                        e.Canceled = false;                                            
                        CurrentViewContext.BkgPackageTypeId = 0;
                        CurrentViewContext.PackageTypeColorCode = String.Empty;
                        base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdBkgPackageType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                    CurrentViewContext.PackageTypeName = txtPackageTypeName.Text;
                    CurrentViewContext.PackageTypeCode = txtPackageTypeCode.Text;
                    Presenter.GetAllBkgPackageTypes(CurrentViewContext.BkgPackageTypeId, String.Empty);
                    grdBkgPackageType.DataSource = CurrentViewContext.lstBkgPackageType;
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

        protected void grdBkgPackageType_SortCommand(object sender, GridSortCommandEventArgs e)
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

        protected void grdBkgPackageType_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlTenantName = editform.FindControl("ddlTenantName") as WclComboBox;                    
                    // BindTenant();
                    ddlTenantName.DataSource = CurrentViewContext.ListTenants;
                    ddlTenantName.DataBind();
                    ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));

                    if (CurrentViewContext.SelectedTenantId > AppConsts.NONE)
                    {
                        ddlTenantName.Enabled = false;
                        ddlTenantName.SelectedValue = CurrentViewContext.SelectedTenantId.ToString();
                    }
                    else
                    {
                        ddlTenant.Enabled = true;
                    }
                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        Int32 BkgPackageId = Convert.ToInt32(editform.GetDataKeyValue("BPT_Id"));
                        RadColorPicker radColorPicker = editform.FindControl("crpPieChartColor") as RadColorPicker;
                        if (!CurrentViewContext.lstBkgPackageType.IsNullOrEmpty())
                        {
                            System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(CurrentViewContext.lstBkgPackageType.Where(x => x.BPT_Id == BkgPackageId).FirstOrDefault().BPT_Color);
                            radColorPicker.SelectedColor = col;
                        }
                    }
                }
                
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    TextBox txt = (TextBox)e.Item.FindControl("txtcolor") as TextBox;
                    string str = dataItem["PackageTypeCode"].Text;                    
                    foreach (var a in CurrentViewContext.lstBkgPackageType)
                    {
                        if (a.BPT_Code == str)
                        {
                            txt.Style.Add("background-color", a.BPT_Color);                            
                            txt.Enabled = false;
                            txt.Style.Add("width", "25px"); 
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

        protected void grdBkgPackageType_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {

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

        protected void crpPieChartColor_ColorChanged(object sender, EventArgs e)
        {
            GridEditFormInsertItem insertItem = (sender as RadColorPicker).NamingContainer as GridEditFormInsertItem;
            CurrentViewContext.PackageTypeColorCode = ColorTranslator.ToHtml((sender as RadColorPicker).SelectedColor);
            if (CurrentViewContext.SelectedTenantId > AppConsts.NONE)
            {
                List<BkgPackageType> lstPackageType = new List<BkgPackageType>();
                lstPackageType = Presenter.GetAllBkgPackageTypes(CurrentViewContext.BkgPackageTypeId, CurrentViewContext.PackageTypeColorCode);
                if (!lstBkgPackageType.IsNullOrEmpty() && lstBkgPackageType.Count > AppConsts.NONE)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('This color code is already used by another PackageType in selected Institute. Please try different color code');", true);
                }

            }
        }
        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdBkgPackageType.MasterTableView.SortExpressions.Clear();
            grdBkgPackageType.CurrentPageIndex = 0;
            grdBkgPackageType.MasterTableView.CurrentPageIndex = 0;
            grdBkgPackageType.MasterTableView.IsItemInserted = false;
            grdBkgPackageType.MasterTableView.ClearEditItems();
            grdBkgPackageType.Rebind();
            if (ViewState["SortExpression"] != null)
            {
                ViewState["SortExpression"] = null;
            }
        }

        private void ResetControls()
        {
            txtPackageTypeName.Text = String.Empty;
            txtPackageTypeCode.Text = String.Empty;            
            CurrentViewContext.PackageTypeName = String.Empty;
            CurrentViewContext.PackageTypeCode = String.Empty;
            CurrentViewContext.PackageTypeColorCode = String.Empty;

        }

        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GridEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as GridEditFormInsertItem;
                //CurrentViewContext.SelectedTenantId = Convert.ToInt32((sender as WclComboBox).SelectedValue);
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
}