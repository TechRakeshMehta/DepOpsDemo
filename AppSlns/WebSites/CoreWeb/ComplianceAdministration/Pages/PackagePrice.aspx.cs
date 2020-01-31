using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using Entity.ClientEntity;
using System.Collections.Generic;
using CoreWeb.Shell;
using CoreWeb.Shell.MasterPages;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class PackagePrice : BaseWebPage, IPackagePriceView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private PackagePricePresenter _presenter=new PackagePricePresenter();
        private String _viewType;
        private PriceContract _viewContract;
        private Int32 _tenantid;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        
        public PackagePricePresenter Presenter
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
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for current user's id.
        /// </value>
        Int32 IPackagePriceView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IPackagePriceView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 TenantId
        {
            get { return (Int32)(ViewState["TenantId"]); }
            set { ViewState["TenantId"] = value; }
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

        public PriceContract ViewContract
        {
            get
            {
                if (_viewContract == null)
                {
                    _viewContract = new PriceContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// ListDeptProgramPackagePriceAdjustment
        /// </summary>
        List<PriceContract> IPackagePriceView.ListDeptProgramPackagePriceAdjustment
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String TreeNodeType
        {
            get
            {
                return Convert.ToString(ViewState["TreeNodeType"]);
            }
            set
            {
                ViewState["TreeNodeType"] = value;
            }
        }

        public String TreeNodeValue
        {
            get
            {
                return Convert.ToString(ViewState["TreeNodeValue"]);
            }
            set
            {
                ViewState["TreeNodeValue"] = value;
            }
        }

        public List<Entity.ClientEntity.PriceAdjustment> ListPriceAdjustment
        {
            get;
            set;
        }

        public Decimal Price
        {
            get
            {
                if (String.IsNullOrEmpty(txtPrice.Text.Trim()))
                {
                    return 0;
                }
                else { return Convert.ToDecimal(txtPrice.Text.Trim()); }
            }
            set
            {
                txtPrice.Text = value.ToString();
            }
        }

        public Decimal TotalPrice
        {
            get
            {
                if (String.IsNullOrEmpty(txtTotalPrice.Text.Trim()))
                {
                    return 0;
                }
                else { return Convert.ToDecimal(txtTotalPrice.Text.Trim()); }
            }
            set
            {
                txtTotalPrice.Text = value.ToString();
            }
        }

        public Decimal? RushOrderAdditionalPrice
        {
            get
            {
                if (String.IsNullOrEmpty(txtRushOrderAdditionalPrice.Text.Trim())) { return null; }
                else { return Convert.ToDecimal(txtRushOrderAdditionalPrice.Text.Trim()); }
            }
            set
            {
                txtRushOrderAdditionalPrice.Text = value.ToString();
            }
        }

        public Decimal PriceAdjustmentValue
        {
            get;
            set;
        }

        public Int32 SelectedPriceAdjustmentID
        {
            get;
            set;
        }

        public Int32 ID
        {
            get
            {
                return Convert.ToInt32(ViewState["ID"]);
            }
            set
            {
                ViewState["ID"] = value;
            }
        }

        public Int32 ParentID
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentID"]);
            }
            set
            {
                ViewState["ParentID"] = value;
            }
        }

        public Int32 MappingID
        {
            get
            {
                return Convert.ToInt32(ViewState["MappingID"]);
            }
            set
            {
                ViewState["MappingID"] = value;
            }
        }

        public Int32 ParentSubscriptionID
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentSubscriptionID"]);
            }
            set
            {
                ViewState["ParentSubscriptionID"] = value;
            }
        }

        public Int32 ComplianceCategoryID
        {
            get
            {
                return Convert.ToInt32(ViewState["ComplianceCategoryID"]);
            }
            set
            {
                ViewState["ComplianceCategoryID"] = value;
            }
        }

        public Boolean IsPriceDisabled
        {
            get;
            set;
        }

        public Boolean IsShowMessage
        {
            get;
            set;
        }

        public String PermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["PermissionCode"]);
            }
            set
            {
                ViewState["PermissionCode"] = value;
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Page_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                    CurrentViewContext.ID = Convert.ToInt32(Request.QueryString["Id"]);
                    CurrentViewContext.ParentID = Convert.ToInt32(Request.QueryString["ParentID"]);
                    CurrentViewContext.MappingID = Convert.ToInt32(Request.QueryString["MappingID"]);
                    CurrentViewContext.TreeNodeType = Convert.ToString(Request.QueryString["TreeNodeType"]);
                    CurrentViewContext.TreeNodeValue = Convert.ToString(Request.QueryString["TreeNodeValue"]);
                    CurrentViewContext.PermissionCode = Request.QueryString["PermissionCode"];
                   
                    //Get Item, Catagory and subscription IDs for selected ITEM 
                    String[] NodeItemIds = Request.QueryString["NodeID"].Split('_');
                    if (NodeItemIds[1] == "ITM")
                    {
                        CurrentViewContext.ItemID = Convert.ToInt32(NodeItemIds[2]);
                        CurrentViewContext.ItmCatID = Convert.ToInt32(NodeItemIds[3]);
                        CurrentViewContext.ItmSubsID = Convert.ToInt32(NodeItemIds[4]);
                    }
                    String[] nodeID = Request.QueryString["ParentNodeID"].Split('_');
                    //Use ParentSubscriptionID and ComplianceCategoryID for Item Price node screen if ID and ParentID both are 0
                    if (nodeID.IndexExists(AppConsts.ONE))
                        CurrentViewContext.ParentSubscriptionID = Convert.ToInt32(nodeID[AppConsts.ONE]);
                    if (nodeID.IndexExists(AppConsts.TWO))
                        CurrentViewContext.ComplianceCategoryID = Convert.ToInt32(nodeID[AppConsts.TWO]);
                    BindControls();
                }
                Presenter.OnViewLoaded();

                //To check if admin logged in or not
                if (!Presenter.IsAdminLoggedIn())
                {
                    DisableControls();
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
        /// Page_PreRenderComplete event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                EnableDisablePriceByPriceModel();

                //To check if admin logged in or not
                if (!Presenter.IsAdminLoggedIn())
                {
                    DisableControls();
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
        /// Add button Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    divAddForm.Visible = true;
        //    ResetControls();
        //}

        /// <summary>
        /// Sets the list of filters to be displayed in the grid. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPriceAdjustmentData_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdPriceAdjustmentData.FilterMenu;
            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// To set DataSource of grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPriceAdjustmentData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            /* Presenter.GetCompliancePackages();

             if (CurrentViewContext.CompliancePackages.Count > 0)
             {
                 grdPackage.Visible = true;
                 lblTitle.Visible = true;
                 grdPackage.DataSource = CurrentViewContext.CompliancePackages;
                 grdPackage.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(TenantId);
             }
             else
             {
                 grdPackage.Visible = false;
                 lblTitle.Visible = false;
             } */
            try
            {
                Presenter.GetPriceAdjustmentData();
                grdPriceAdjustmentData.DataSource = CurrentViewContext.ListDeptProgramPackagePriceAdjustment;
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
        /// handle the commands fired from the grid
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdPriceAdjustmentData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    CurrentViewContext.ViewContract.ID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
                    if (Presenter.DeletePriceAdjustmentData())
                    {
                        base.ShowSuccessMessage("Price Adjustment deleted successfully.");
                        BindPrice();
                    }
                    //else
                    //{
                    //    base.ShowInfoMessage("You can not remove Price Adjustment as it is in use.");
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

        /// <summary>
        /// grdPriceAdjustmentData_InsertCommand event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPriceAdjustmentData_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                WclComboBox ddlPriceAdjustment = e.Item.FindControl("ddlPriceAdjustment") as WclComboBox;
                WclNumericTextBox txtPriceAdjustmentValue = e.Item.FindControl("txtPriceAdjustmentValue") as WclNumericTextBox;

                //CurrentViewContext.ViewContract.RenewalTerm = Convert.ToInt16((e.Item.FindControl("txtRenewalTerm") as WclTextBox).Text.Trim());
                if (!String.IsNullOrEmpty(txtPriceAdjustmentValue.Text.Trim()))
                    CurrentViewContext.PriceAdjustmentValue = Convert.ToDecimal((txtPriceAdjustmentValue).Text.Trim());

                if (ddlPriceAdjustment.SelectedValue != String.Empty)
                {
                    CurrentViewContext.SelectedPriceAdjustmentID = Convert.ToInt32(ddlPriceAdjustment.SelectedValue);
                }

                Presenter.SavePriceAdjustmentDetail();
                if (String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Price Adjustment saved successfully.");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    BindPrice();
                }
                else
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
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
        /// grdPriceAdjustmentData_UpdateCommand event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPriceAdjustmentData_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                WclComboBox ddlPriceAdjustment = e.Item.FindControl("ddlPriceAdjustment") as WclComboBox;
                WclNumericTextBox txtPriceAdjustmentValue = e.Item.FindControl("txtPriceAdjustmentValue") as WclNumericTextBox;

                //CurrentViewContext.ViewContract.RenewalTerm = Convert.ToInt16((e.Item.FindControl("txtRenewalTerm") as WclTextBox).Text.Trim());
                if (!String.IsNullOrEmpty(txtPriceAdjustmentValue.Text.Trim()))
                    CurrentViewContext.ViewContract.PriceAdjustmentValue = Convert.ToDecimal((txtPriceAdjustmentValue).Text.Trim());

                if (ddlPriceAdjustment.SelectedValue != String.Empty)
                {
                    CurrentViewContext.ViewContract.PriceAdjustmentID = Convert.ToInt32(ddlPriceAdjustment.SelectedValue);
                }

                CurrentViewContext.ViewContract.ID = Convert.ToInt16((e.Item.FindControl("txtID") as WclTextBox).Text.Trim());

                Presenter.UpdatePriceAdjustmentDetail();

                if (String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Price Adjustment saved successfully.");
                    BindPrice();
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    //base.ShowSuccessMessage("Price Adjustment saved successfully.");
                }

                //else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                //{
                //    e.Canceled = false;
                //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                //    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        /// <summary>
        /// grdPriceAdjustmentData_ItemCreated event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPriceAdjustmentData_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox ddlPriceAdjustment = (WclComboBox)editform.FindControl("ddlPriceAdjustment");

                    Presenter.GetPriceAdjustment();
                    ddlPriceAdjustment.DataSource = CurrentViewContext.ListPriceAdjustment;
                    //ddlPriceAdjustment.Items.Insert(0, new RadComboBoxItem { Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.ZERO });
                    ddlPriceAdjustment.DataBind();


                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        Int32 priceAdjustmentID = Convert.ToInt32(editform.GetDataKeyValue("PriceAdjustmentID"));
                        if (priceAdjustmentID != 0)
                            ddlPriceAdjustment.SelectedValue = priceAdjustmentID.ToString();
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

        /// <summary>
        /// Node grid ItemDataBound event to hide delete button conditionally
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPriceAdjustmentData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    DeptProgramMapping deptProgramMapping = e.Item.DataItem as DeptProgramMapping;
                    GridDataItem dataItem = e.Item as GridDataItem;

                    //If client admin logged in and permissions are ReadOnly and NoAccess type then hide delete buton
                    if (!Presenter.IsAdminLoggedIn())
                    {
                        if (CurrentViewContext.PermissionCode == LkpPermission.ReadOnly.GetStringValue()
                             || CurrentViewContext.PermissionCode == LkpPermission.NoAccess.GetStringValue())
                        {
                            (e.Item as GridDataItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
                            (e.Item as GridDataItem)["EditCommandColumn"].Controls[AppConsts.NONE].Visible = false;
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

        /// <summary>
        /// Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarPrice_SaveClick(object sender, EventArgs e)
        {
            /* CurrentViewContext.ViewContract.PackageName = txtPackageName.Text.Trim();
            CurrentViewContext.ViewContract.Description = txtPkgDescription.Text.Trim();
            CurrentViewContext.ViewContract.PackageLabel = txtPackageLabel.Text.Trim();
            CurrentViewContext.ViewContract.ScreenLabel = txtScreenLabel.Text.Trim();
            CurrentViewContext.ViewContract.State = chkActive.Checked;
            CurrentViewContext.ViewContract.ExceptionDescription = txtPkgExceptionDesc.Text.Trim();
            CurrentViewContext.ViewContract.ExplanatoryNotes = txtPkgNotes.Text.Trim();
            */

            /*if (!String.IsNullOrEmpty(txtPrice.Text.Trim()))
            {
                CurrentViewContext.Price = Convert.ToDecimal(txtPrice.Text.Trim());
            }
            if (!String.IsNullOrEmpty(txtPriceAdjustmentValue.Text.Trim()))
            {
                CurrentViewContext.PriceAdjustmentValue = Convert.ToDecimal(txtPriceAdjustmentValue.Text.Trim());
            }
            CurrentViewContext.SelectedPriceAdjustmentID = Convert.ToInt32(ddlPriceAdjustment.SelectedValue);
             */
            try
            {
                Presenter.SavePriceAdjustmentDetail();

                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    base.ShowSuccessMessage("Price saved successfully.");
                    //divAddForm.Visible = false;
                    //ResetControls();
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    BindPrice();
                    grdPriceAdjustmentData.Rebind();
                }
                else
                {
                    base.ShowInfoMessage(ErrorMessage);
                    //divAddForm.Visible = true;
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
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarPrice_CancelClick(object sender, EventArgs e)
        {
            Presenter.GetPrice();
            //divAddForm.Visible = false;
            //ResetControls();
        }

        protected void ddlPriceAdjustment_DataBound(object sender, EventArgs e)
        {
            //ddlPriceAdjustment.Items.Insert(0, new DropDownListItem("--Select--"));
            //ddlPriceAdjustment.Items.Insert(0, new RadComboBoxItem { Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.ZERO });
        }

        #endregion

        #region Methods

        /// <summary>
        /// To reset Controls
        /// </summary>
        private void ResetControls()
        {
            //txtPrice.Text = String.Empty;
            //txtPriceAdjustmentValue.Text = String.Empty;
            //ddlPriceAdjustment.SelectedIndex = AppConsts.NONE;
        }

        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            txtName.Text = CurrentViewContext.TreeNodeValue;
            BindPrice();
            //BindPriceAdjustment();
        }

        /// <summary>
        /// To get Price Adjustment List and bind Price Adjustment dropdown
        /// </summary>
        private void BindPriceAdjustment()
        {
            //Presenter.GetPriceAdjustment();
            //ddlPriceAdjustment.DataSource = CurrentViewContext.ListPriceAdjustment;
            //ddlPriceAdjustment.DataBind();
        }

        /// <summary>
        /// To bind Price
        /// </summary>
        private void BindPrice()
        {
            Presenter.GetPrice();
        }

        /// <summary>
        /// To enable disable Price By Price Model
        /// </summary>
        private Boolean EnableDisablePriceByPriceModel()
        {
            Presenter.EnableDisablePriceByPriceModel();
            txtPrice.Enabled = !(CurrentViewContext.IsPriceDisabled);
            txtRushOrderAdditionalPrice.Enabled = !(CurrentViewContext.IsPriceDisabled);
            //Show hide div
            //Check if tree Node Type is Subscription/Package, Category or Item
            Presenter.ShowMessage();
            divPrice.Visible = !(CurrentViewContext.IsShowMessage);

            if (CurrentViewContext.TreeNodeType.Equals(RuleSetTreeNodeType.Category))
            {
                if (CurrentViewContext.IsShowMessage)
                {
                    base.ShowInfoMessage("Price is defined at Package level.");
                }
            }
            else if (CurrentViewContext.TreeNodeType.Equals(RuleSetTreeNodeType.Item))
            {
                if (CurrentViewContext.IsShowMessage)
                {
                    base.ShowInfoMessage("Price is defined at Package or Category level.");
                }
            }

            divContent.Visible = !(CurrentViewContext.IsPriceDisabled);
            divButton.Visible = !(CurrentViewContext.IsPriceDisabled);

            return !(CurrentViewContext.IsPriceDisabled);
        }

        /// <summary>
        /// To disable controls as per permissions
        /// </summary>
        private void DisableControls()
        {
            if (CurrentViewContext.PermissionCode == LkpPermission.ReadOnly.GetStringValue()
                || CurrentViewContext.PermissionCode == LkpPermission.NoAccess.GetStringValue())
            {
                fsucCmdBarPrice.SaveButton.Enabled = false;
                fsucCmdBarPrice.CancelButton.Enabled = false;
                grdPriceAdjustmentData.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                txtPrice.Enabled = false;
                txtRushOrderAdditionalPrice.Enabled = false;
            }
        }

        #endregion


        public int ItemID
        {
            get;
            set;
        }

        public int ItmCatID
        {
            get;
            set;
        }

        public int ItmSubsID
        {
            get;
            set;
        }
    }
}

