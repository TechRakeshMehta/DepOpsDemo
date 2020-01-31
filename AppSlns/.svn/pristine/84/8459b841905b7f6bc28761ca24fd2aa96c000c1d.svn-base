using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using Telerik.Web.UI;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{

    public partial class EditFeeItemDetail : BaseUserControl, IManageSvcItemFeeItemsView
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
            get;
            set;
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

        public Int32 FeeItemId
        {
            get
            {
                if (!ViewState["FeeItemId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["FeeItemId"]);
                }
                return 0;
            }
            set
            {
                ViewState["FeeItemId"] = value;
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

        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    SetEditFeeItemBlock();
                    ApplyActionLevelPermission(ActionCollection, "Manage Fee Items");
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

        protected void CmdBarEdit_Click(object sender, EventArgs e)
        {
            try
            {
                SetControlsEnableDisableProperty(true);
                CurrentViewContext.SuccessMessage = String.Empty;
                CurrentViewContext.InfoMessage = String.Empty;
                CurrentViewContext.ErrorMessage = String.Empty;
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

        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.FeeItemTypeId = Convert.ToInt32(ddlFeeItemType.SelectedValue);
                CurrentViewContext.ViewContract.FeeItemName = txtFeeItemName.Text.Trim();
                //CurrentViewContext.ViewContract.FeeItemLabel = txtFeeItemLabel.Text.Trim();
                CurrentViewContext.ViewContract.FeeItemDescription = txtDescription.Text.Trim();

                CurrentViewContext.ViewContract.FixedFeeAmount = (txtFixedTypeAmount.Text.IsNullOrEmpty() ? (Decimal?)null
                    : Convert.ToDecimal(txtFixedTypeAmount.Text.Trim()));

                Presenter.UpdatePackageServiceItemFeeRecord(FeeItemId);
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
                    SetEditFeeItemBlock();
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

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetControlsEnableDisableProperty(false);
                CurrentViewContext.SuccessMessage = String.Empty;
                CurrentViewContext.InfoMessage = String.Empty;
                CurrentViewContext.ErrorMessage = String.Empty;
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
        protected void ddlFeeItemType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbFeeItemType = sender as WclComboBox;

            if (cmbFeeItemType.SelectedItem.Text.ToLower().Equals("fixed fee"))
                divFixedTypeAmount.Visible = true;
            else
                divFixedTypeAmount.Visible = false;
        }

        #endregion

        #region Methods

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
        }

        private void SetEditFeeItemBlock()
        {
            Presenter.GetFeeItemByID();
            Presenter.GetServiceItemFeeTypeList();
            BindCombo(ddlFeeItemType, CurrentViewContext.lstServiceItemFeeTypes);
            txtDescription.Text =  txtFeeItemName.Text = txtFixedTypeAmount.Text = "";
            txtDescription.Text = CurrentViewContext.packageServiceItemFee.PSIF_Description;
            //txtFeeItemLabel.Text = CurrentViewContext.packageServiceItemFee.PSIF_Label;
            txtFeeItemName.Text = CurrentViewContext.packageServiceItemFee.PSIF_Name;
            ddlFeeItemType.SelectedValue = CurrentViewContext.packageServiceItemFee.PSIF_ServiceItemFeeType.ToString();

            if (ddlFeeItemType.SelectedItem.Text.ToLower().Equals("fixed fee"))
                txtFixedTypeAmount.Text = CurrentViewContext.packageServiceItemFee.ServiceItemFeeRecords.FirstOrDefault().SIFR_Amount.ToString();            

            SetControlsEnableDisableProperty(false);            
        }

        private void SetControlsEnableDisableProperty(Boolean enable)
        {
            txtDescription.Enabled = txtFeeItemName.Enabled = txtFixedTypeAmount.Enabled = ddlFeeItemType.Enabled =  enable;

            // if Fee Records exists then not allowed to update type txtFeeItemLabel.Enabled =
            if (CurrentViewContext.packageServiceItemFee.IsNull())
                Presenter.GetFeeItemByID();
            if (CurrentViewContext.packageServiceItemFee.ServiceItemFeeRecords.Any(condition => condition.SIFR_IsDeleted == false))
                ddlFeeItemType.Enabled = false;            
            
            btnCancel.Visible = btnSave.Visible = enable;

            btnEdit.Visible = !enable;

            if (ddlFeeItemType.SelectedItem.Text.ToLower().Equals("fixed fee"))
                divFixedTypeAmount.Visible = true;
        }

        private void ClearControls()
        {
            txtDescription.Text =  txtFeeItemName.Text = txtFixedTypeAmount.Text = "";
            ddlFeeItemType.SelectedValue = AppConsts.ZERO.ToString();
            //txtFeeItemLabel.Text =
        }


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
                                if (x.FeatureAction.CustomActionId == "EditFeeItem")
                                {
                                    btnSave.Enabled = false;
                                }                               
                                break;
                            }
                    }

                });
            }
        }

        #endregion

        #endregion
    }
}
   