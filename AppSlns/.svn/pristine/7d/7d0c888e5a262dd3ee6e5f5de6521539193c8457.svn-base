using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgSetup.Views;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.UserControl
{
    public partial class ManageSvcItemCustomForms : BaseUserControl, IManageSvcItemCustomFormsView
    {
        #region Private Variables

        private ManageSvcItemCustomFormsPresenter _presenter = new ManageSvcItemCustomFormsPresenter();

        #endregion

        #region Public Properties

        public ManageSvcItemCustomFormsPresenter Presenter
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

        public IManageSvcItemCustomFormsView CurrentViewContext
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


        Int32 IManageSvcItemCustomFormsView.currentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        string IManageSvcItemCustomFormsView.ErrorMessage
        {
            get;
            set;
        }

        string IManageSvcItemCustomFormsView.InfoMessage
        {
            get;
            set;
        }

        string IManageSvcItemCustomFormsView.SuccessMessage
        {
            get;
            set;
        }

        List<Entity.CustomForm> IManageSvcItemCustomFormsView.lstCustomFormSupplementary { get; set; }

        List<PkgServiceItemCustomFormMappingDetails> IManageSvcItemCustomFormsView.lstPkgServiceItemCustomFormMappingDetails { get; set; }

        Int32? IManageSvcItemCustomFormsView.SelectedCustomFormId
        {

            get
            {
                if (!cmbCustomForm.SelectedValue.IsNullOrEmpty())
                    if (Convert.ToInt32(cmbCustomForm.SelectedValue) == -1)
                    {
                        return null;
                    }
                    else
                        return Convert.ToInt32(cmbCustomForm.SelectedValue);
                return AppConsts.NONE;
            }
            set
            {
                cmbCustomForm.SelectedValue = Convert.ToString(value);
            }
        }

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Presenter.OnViewInitialized();
            ApplyActionLevelPermission(ActionCollection, "Manage Custom Forms");
            if (!IsPostBack)
                grdManageSvcItemCustomForms.Rebind();
        }

        #endregion

        #region Grid Events

        protected void grdManageSvcItemCustomForms_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetPkgServiceItemCustomFormMappingDetails();
            if (CurrentViewContext.lstPkgServiceItemCustomFormMappingDetails.IsNotNull())
            {
                grdManageSvcItemCustomForms.DataSource = CurrentViewContext.lstPkgServiceItemCustomFormMappingDetails;
            }
            else
            {
                grdManageSvcItemCustomForms.DataSource = new List<PkgServiceItemCustomFormMappingDetails>();
            }
           
        }

        protected void grdManageSvcItemCustomForms_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 bkgSvcItemFormMappingId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BSIFM_ID"));
                Presenter.DeleteBkgSvcItemFormMapping(bkgSvcItemFormMappingId);
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


        #region Button Events

        protected void btnAddCustomForm_Click(object sender, EventArgs e)
        {
            try
            {
                ShowAddItemBlock();
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
                
                Presenter.SaveBkgSvcItemFormMapping();
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
                    grdManageSvcItemCustomForms.Rebind();
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
            cmbBox.Items.Insert(AppConsts.NONE, new RadComboBoxItem { Selected = true, Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.MINUS_ONE.ToString() });
        }

        private void ShowAddItemBlock()
        {
            cmbCustomForm.Items.Clear();
            Presenter.GetSupplCustomFrmsNotMappedToSvcItem();
            BindCombo(cmbCustomForm, CurrentViewContext.lstCustomFormSupplementary);
            divButtonSave.Visible = true;
            divCustomForm.Visible = true;
        }

        private void ClearControls()
        {
            cmbCustomForm.SelectedValue = AppConsts.MINUS_ONE.ToString();
            divCustomForm.Visible = false;
            divButtonSave.Visible = false;
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
                                if (x.FeatureAction.CustomActionId == "SaveCustomForm")
                                {
                                    btnSave.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteCustomForm")
                                {
                                    grdManageSvcItemCustomForms.MasterTableView.GetColumn("DeleteColumn").Display = false;
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