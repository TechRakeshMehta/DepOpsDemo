using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{

    public partial class ManageSvcItemFeeItems : BaseUserControl, IManageSvcItemFeeItemsView
    {

        #region Private Variables

        private ManageSvcItemFeeItemsPresenter _presenter = new ManageSvcItemFeeItemsPresenter();
        private FeeItemContract _viewContract;
        
        #endregion

        #region Public Properties

        public ManageSvcItemFeeItemsPresenter Presenter
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

        public Int32 PackageServiceItemID
        {
            get
            {
                if (!ViewState["PackageServiceItemID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["PackageServiceItemID"]);
                }
                return 0;
            }
            set
            {
                ViewState["PackageServiceItemID"] = value;
            }
        }

        public IManageSvcItemFeeItemsView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }
        #endregion

        #region Private Properties
        FeeItemContract IManageSvcItemFeeItemsView.ViewContract
        {
            get
            {
                if (_viewContract == null)
                {
                    _viewContract = new FeeItemContract();
                }

                return _viewContract;
            }

        }


        List<LocalFeeItemsInfo> IManageSvcItemFeeItemsView.lstPackageServiceItemFee { get; set; }

        List<lkpServiceItemFeeType> IManageSvcItemFeeItemsView.lstServiceItemFeeTypes { get; set; }

        

        int IManageSvcItemFeeItemsView.currentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        string IManageSvcItemFeeItemsView.ErrorMessage
        {
            get;
            set;
        }

        string IManageSvcItemFeeItemsView.InfoMessage
        {
            get;
            set;
        }

        string IManageSvcItemFeeItemsView.SuccessMessage
        {
            get;
            set;
        }

        PackageServiceItemFee IManageSvcItemFeeItemsView.packageServiceItemFee { get; set; }

        Int32 IManageSvcItemFeeItemsView.FeeItemId { get; set; }

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Presenter.OnViewInitialized();
            ApplyActionLevelPermission(ActionCollection, "Manage Fee Items");
        }

        #endregion

        #region Grid Events

        protected void grdManageSvcItemFeeItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetPackageSvcItemFeeList();
            grdManageSvcItemFeeItems.DataSource = CurrentViewContext.lstPackageServiceItemFee;

        }

        protected void grdManageSvcItemFeeItems_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                //lblServiceItemFeeRecordAmount
                //if (e.item is griddataitem && e.item.dataitem != null)
                //{
                //    packageserviceitemfee pkgsvcitemfee = e.item.dataitem as packageserviceitemfee;

                //    label lblserviceitemfeerecordamount = e.item.findcontrol("lblserviceitemfeerecordamount") as label;
                //    decimal? amount = pkgsvcitemfee.serviceitemfeerecords.where(obj => obj.sifr_feeeitemid == pkgsvcitemfee.psif_id && obj.sifr_isdeleted == false).select(obj => obj.sifr_amount).firstordefault();

                //    if (pkgsvcitemfee.lkpserviceitemfeetype.sift_name.tolower().equals("fixed fee") && pkgsvcitemfee.serviceitemfeerecords.count > 0 && amount.hasvalue)
                //    {
                //        lblserviceitemfeerecordamount.text = amount.value.tostring();
                //        // grdmanagesvcitemfeeitems.mastertableview.getcolumn("siframount").sortexpression = amount.value.tostring();
                //    }
                //    else
                //        lblserviceitemfeerecordamount.text = "";
                //}
                //if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                //{
                //    GridEditFormItem editform = (e.Item as GridEditFormItem);
                //    WclComboBox cmbFeeItemType = editform.FindControl("ddlFeeItemType") as WclComboBox;
                //    Presenter.GetServiceItemFeeTypeList();
                //    cmbFeeItemType.DataSource = CurrentViewContext.lstServiceItemFeeTypes;
                //    cmbFeeItemType.DataBind();
                //    if (!(e.Item.DataItem is GridInsertionObject))
                //    {
                //        PackageServiceItemFee pkgSvcItemFee = e.Item.DataItem as PackageServiceItemFee;

                //        if (pkgSvcItemFee != null)
                //        {
                //            CurrentViewContext.ViewContract.FeeItemTypeId = pkgSvcItemFee.PSIF_ServiceItemFeeType;
                //            Panel panel = editform.FindControl("pnlReviewer") as Panel;
                //            if (!CurrentViewContext.ViewContract.FeeItemTypeId.IsNullOrEmpty())
                //            {
                //                cmbFeeItemType.SelectedValue = CurrentViewContext.ViewContract.FeeItemTypeId.ToString();

                //                // if Fee Records exists then not allowed to update type
                //                if (pkgSvcItemFee.ServiceItemFeeRecords.Count > 0)
                //                    cmbFeeItemType.Enabled = false;
                //                else
                //                    cmbFeeItemType.Enabled = true;

                //                if (cmbFeeItemType.SelectedItem.Text.ToLower().Equals("fixed fee"))
                //                {
                //                    ShowHideContentArea(panel, true);
                //                    WclNumericTextBox txtFixedTypeAmount = editform.FindControl("txtFixedTypeAmount") as WclNumericTextBox;
                //                    txtFixedTypeAmount.Text = pkgSvcItemFee.ServiceItemFeeRecords.Where(obj => obj.SIFR_FeeeItemId == pkgSvcItemFee.PSIF_ID && obj.SIFR_IsDeleted==false).Select(obj => obj.SIFR_Amount).FirstOrDefault().ToString();
                //                }
                //                else
                //                    ShowHideContentArea(panel, false);
                //            }
                //        }
                //    }
                //}
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdManageSvcItemFeeItems_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.ViewContract.FeeItemTypeId = Convert.ToInt32((e.Item.FindControl("ddlFeeItemType") as WclComboBox).SelectedValue);
                CurrentViewContext.ViewContract.FeeItemName = (e.Item.FindControl("txtFeeItemName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.FeeItemLabel = (e.Item.FindControl("txtFeeItemLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.FeeItemDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();

                CurrentViewContext.ViewContract.FixedFeeAmount = ((e.Item.FindControl("txtFixedTypeAmount") as WclNumericTextBox).Text.IsNullOrEmpty() ? (Decimal?)null
                   : Convert.ToDecimal((e.Item.FindControl("txtFixedTypeAmount") as WclNumericTextBox).Text.Trim()));

                Presenter.SavePackageServiceItemFeeRecord();
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdManageSvcItemFeeItems_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 pkgSvcItemFeeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PSIF_ID"));
                CurrentViewContext.ViewContract.FeeItemTypeId = Convert.ToInt32((e.Item.FindControl("ddlFeeItemType") as WclComboBox).SelectedValue);
                CurrentViewContext.ViewContract.FeeItemName = (e.Item.FindControl("txtFeeItemName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.FeeItemLabel = (e.Item.FindControl("txtFeeItemLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.FeeItemDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();

                CurrentViewContext.ViewContract.FixedFeeAmount = ((e.Item.FindControl("txtFixedTypeAmount") as WclNumericTextBox).Text.IsNullOrEmpty() ? (Decimal?)null
                    : Convert.ToDecimal((e.Item.FindControl("txtFixedTypeAmount") as WclNumericTextBox).Text.Trim()));

                Presenter.UpdatePackageServiceItemFeeRecord(pkgSvcItemFeeId);
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdManageSvcItemFeeItems_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 pkgSvcItemFeeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PSIF_ID"));
                Presenter.DeletePackageServiceItemFeeData(pkgSvcItemFeeId);
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Dropdown Events
        protected void ddlFeeItemType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbFeeItemType = sender as WclComboBox;           

            if (cmbFeeItemType.SelectedItem.Text.ToLower().Equals("fixed fee"))
                divFixedTypeAmount.Visible = true;
            else
                divFixedTypeAmount.Visible =  false;
        }

        #endregion


        #region Button Events
        protected void btnAddFeeItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowAddFeeItemBlock();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.FeeItemTypeId = Convert.ToInt32(ddlFeeItemType.SelectedValue);
                CurrentViewContext.ViewContract.FeeItemName = txtFeeItemName.Text.Trim();
                //CurrentViewContext.ViewContract.FeeItemLabel = txtFeeItemLabel.Text.Trim();
                CurrentViewContext.ViewContract.FeeItemDescription = txtDescription.Text.Trim();

                CurrentViewContext.ViewContract.FixedFeeAmount = (txtFixedTypeAmount.Text.IsNullOrEmpty() ? (Decimal?)null
                   : Convert.ToDecimal(txtFixedTypeAmount.Text.Trim()));

                Presenter.SavePackageServiceItemFeeRecord();
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    ClearControls();
                    grdManageSvcItemFeeItems.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearControls();
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

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
        }

        private void ShowAddFeeItemBlock()
        {
            Presenter.GetServiceItemFeeTypeList();
            BindCombo(ddlFeeItemType, CurrentViewContext.lstServiceItemFeeTypes);
            divButtonSave.Visible = true;
            divFeeItem.Visible = true;
        }

        private void ClearControls()
        {
            txtDescription.Text = txtFeeItemName.Text = txtFixedTypeAmount.Text = "";
            ddlFeeItemType.SelectedValue = AppConsts.ZERO.ToString();
            divFeeItem.Visible = false;
            divFixedTypeAmount.Visible = false;
            divButtonSave.Visible = false;
        }

        //private void ShowHideContentArea(Panel panel, bool visibility)txtFeeItemLabel.Text = 
        //{
        //    HtmlGenericControl divFixedTypeAmount = (HtmlGenericControl)panel.FindControl("divFixedTypeAmount");
        //    if (divFixedTypeAmount != null)
        //    {
        //        divFixedTypeAmount.Visible = visibility;
        //    }
        //}

        #region Apply Permissions

        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
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
                                if (x.FeatureAction.CustomActionId == "SaveFeeItem")
                                {
                                    btnAddFeeItem.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteFeeItem")
                                {
                                    grdManageSvcItemFeeItems.MasterTableView.GetColumn("DeleteColumn").Display = false;
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

    }
}