#region NameSpaces

#region system defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

#region Project Specific
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Entity.ClientEntity;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageServiceItem : BaseUserControl, IManageServiceItemView
    {

        #region Variables

        #region Private Variables

        private ManageServiceItemPresenter _presenter = new ManageServiceItemPresenter();
        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties

        public ManageServiceItemPresenter Presenter
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
        /// Gets and sets TenantId
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["TenantId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        /// <summary>
        /// Gets and sets Background package service id.
        /// </summary>
        public Int32 BkgPackageSvcId
        {
            get
            {
                if (!ViewState["BkgPackageSvcId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["BkgPackageSvcId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["BkgPackageSvcId"] = value;
            }
        }

        public Int32 ParentNodeId
        {
            get
            {
                if (!ViewState["ParentNodeId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["ParentNodeId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ParentNodeId"] = value;
            }
        }

        String IManageServiceItemView.ServiceItemName
        {
            get;
            set;

        }

        String IManageServiceItemView.ServiceItemDescription
        {
            get;
            set;
        }

        Int32 IManageServiceItemView.ServiceItemTypeId
        {
            get;
            set;
        }

        String IManageServiceItemView.ServiceItemLabel
        {
            get;
            set;
        }

        List<PackageServiceItem> IManageServiceItemView.ListPackageServiceItem
        {
            get;
            set;
        }
        List<lkpServiceItemType> IManageServiceItemView.ListServiceItemType
        {
            get;
            set;
        }

        Int32 IManageServiceItemView.PSI_ID
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
        public String InfoMessage
        {
            get;
            set;
        }

        public Int32 BackgroundServiceId
        {
            get
            {
                if (!ViewState["BackgroundServiceId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["BackgroundServiceId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["BackgroundServiceId"] = value;
            }
        }
        Int32 IManageServiceItemView.AttributeGroupId
        {
            get;
            set;
        }
        Int32 IManageServiceItemView.GlobalFeeItemId
        {
            get;
            set;
        }
        Int32? IManageServiceItemView.ParentServiceItemId
        {
            get;
            set;
        }
        List<BkgSvcAttributeGroup> IManageServiceItemView.ListAttributeGroup
        {
            get;
            set;
        }
        List<PackageServiceItem> IManageServiceItemView.ListParentServiceItem
        {
            get;
            set;
        }

        List<Entity.PackageServiceItemFee> IManageServiceItemView.GlobalPackageServiceFeeItemList
        {
            get;
            set;
        }

        Boolean IManageServiceItemView.IsRequired
        {
            get;
            set;
        }

        Boolean IManageServiceItemView.IsSupplemental
        {
            get;
            set;
        }

        Decimal? IManageServiceItemView.AdditinalOccurencePrice
        {
            get;
            set;
        }
        Int32 IManageServiceItemView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32? IManageServiceItemView.QuantityIncluded
        {
            get
            {
                if (!txtQuantityIncluded.Text.IsNullOrEmpty())
                    return Convert.ToInt32(txtQuantityIncluded.Text);
                return null;
            }
        }

        Int32 IManageServiceItemView.QuantityGroup
        {
            get
            {
                if (!cmbQuantityGroups.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbQuantityGroups.SelectedValue);
                return AppConsts.NONE;
            }
        }


        Int32 IManageServiceItemView.ServiceItemPriceTypeId
        {
            get
            {
                if (!cmbPriceType.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbPriceType.SelectedValue);
                return AppConsts.NONE;
            }
        }

        /// <summary>
        /// Gets and sets Background packageId.
        /// </summary>
        public Int32 PackageId
        {
            get
            {
                if (!ViewState["PackageId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["PackageId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        #region Current View Context
        private IManageServiceItemView CurrentViewContext
        {
            get { return this; }
        }
        #endregion

        public Int32? MinOccurrences
        {
            get
            {
                return txtMinOccurrences.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMinOccurrences.Text);
            }
        }

        public Int32? MaxOccurrences
        {
            get
            {
                return txtMaxOccurrences.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMaxOccurrences.Text);
            }
        }
        #endregion

        #endregion

        #region Events
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ApplyActionLevelPermission(ActionCollection, "Manage Service Item");
                    pnl.Visible = false;
                    BindQuantityGrps();
                    //SetServiceSettings();
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

        #region Grid Related Events
        protected void grdServiceItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetPackageServiceItemList();
                grdServiceItem.DataSource = CurrentViewContext.ListPackageServiceItem;
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

        protected void grdServiceItem_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.PSI_ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PSI_ID"));
                Int32 PSI_BkgPackageHierarchyMappingId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PSI_BkgPackageHierarchyMappingId"));
                if (Presenter.DeletePackageServiceItem(PSI_BkgPackageHierarchyMappingId))
                {
                    grdServiceItem.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                else
                {
                    if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                    {
                        (this.Page as BaseWebPage).ShowErrorMessage(CurrentViewContext.ErrorMessage);
                    }
                    else
                    {
                        (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.InfoMessage);
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

        #region Button Events
        /// <summary>
        /// Event to add Service Item 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarAddServiceItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowAddServiceItemBlock();
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
        /// Event to save data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ServiceItemTypeId = Convert.ToInt32(cmbServiceItemType.SelectedValue);
                CurrentViewContext.AttributeGroupId = Convert.ToInt32(cmbAttributeGroup.SelectedValue);
                if (Convert.ToInt32(cmbParentFeeItem.SelectedValue) != -1)
                {
                    CurrentViewContext.ParentServiceItemId = Convert.ToInt32(cmbParentFeeItem.SelectedValue);
                }
                if (!cmbGlobalFeeItem.SelectedValue.IsNullOrEmpty())
                {
                    CurrentViewContext.GlobalFeeItemId = Convert.ToInt32(cmbGlobalFeeItem.SelectedValue);
                }
                else //Set GlobalFeeItemId to minus one if dropdown selected value is nullor empty -[UAT-831]: WB: Whenever a new Service Item is created without any “Global Fee Item” filled in, by default the value “All County Fee” is saved in the application.
                {
                   CurrentViewContext.GlobalFeeItemId = AppConsts.MINUS_ONE;
                }
                if (!ntxtAddOccPrice.Text.Trim().IsNullOrEmpty())
                {
                    CurrentViewContext.AdditinalOccurencePrice = Convert.ToDecimal(ntxtAddOccPrice.Text.Trim());
                }
                else
                {
                    CurrentViewContext.AdditinalOccurencePrice = null;
                }
                CurrentViewContext.ServiceItemName = txtServiceItemName.Text.Trim();
                //CurrentViewContext.ServiceItemLabel = txtServiceItemLabel.Text.Trim();
                CurrentViewContext.ServiceItemDescription = txtServiceItemDescription.Text.Trim();
                CurrentViewContext.IsRequired = chkRequired.Checked;
                CurrentViewContext.IsSupplemental = chkSupplement.Checked;

                if (!CurrentViewContext.AdditinalOccurencePrice.IsNullOrEmpty() && CurrentViewContext.AdditinalOccurencePrice >= AppConsts.NONE)
                {
                    if (CurrentViewContext.ServiceItemPriceTypeId == 0)
                    {
                        (this.Page as BaseWebPage).ShowInfoMessage("Please select Additional occurence price type.");
                        return;
                    }
                }

                if (Presenter.SaveUpdatePackageServiceItem())
                {
                    ClearControls();
                    grdServiceItem.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                else
                {
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        (this.Page as BaseWebPage).ShowErrorMessage(CurrentViewContext.ErrorMessage);
                    }
                    if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                    {
                        (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.InfoMessage);
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
        /// Event to cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
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

        #region Dropdown Events

        protected void cmbAttributeGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            List<PackageServiceItem> _tempSvcParentItemList = new List<PackageServiceItem>();
            if (!String.IsNullOrEmpty(cmbAttributeGroup.SelectedValue) && cmbAttributeGroup.SelectedValue != AppConsts.ZERO)
            {
                Int32 _selectAttrGrpId = Convert.ToInt32(cmbAttributeGroup.SelectedValue);
                _tempSvcParentItemList = _presenter.GetQuantityGroups(_selectAttrGrpId, this.ParentNodeId, this.TenantId);
                BindQuantityGrps(_tempSvcParentItemList, _selectAttrGrpId);
            }
            ShowHideQuantityIncludedPanel();
            divSettings.Style.Add("display", "none");
            txtMinOccurrences.Text = String.Empty;
            txtMaxOccurrences.Text = String.Empty;
        }

        protected void cmbQuantityGroups_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ShowHideQuantityIncludedPanel();
                SetServiceSettings();
                txtMinOccurrences.Text = String.Empty;
                txtMaxOccurrences.Text = String.Empty;
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

        #region Private Methods

        /// <summary>
        /// Hide the Panel in case selection is reversed to SELECT
        /// </summary>
        private void ShowHideQuantityIncludedPanel()
        {
            if (!String.IsNullOrEmpty(cmbQuantityGroups.SelectedValue) && cmbQuantityGroups.SelectedValue == "-1")
                pnl.Visible = true;
            else
                pnl.Visible = false;
            if (!pnl.Visible)
                txtQuantityIncluded.Text = String.Empty;
        }


        /// <summary>
        /// Manage the Quantity group dropdown
        /// </summary>
        /// <param name="tempSvcParentItemList"></param>
        /// <param name="selectAttrGrpId"></param>
        private void BindQuantityGrps(List<PackageServiceItem> tempSvcParentItemList = null, Int32 selectAttrGrpId = 0)
        {
            if (!tempSvcParentItemList.IsNullOrEmpty() && selectAttrGrpId > 0)
            {
                //tempSvcParentItemList = tempSvcParentItemList.Where(psi => psi.PSI_AttributeGroupId == selectAttrGrpId).ToList();
                tempSvcParentItemList.Insert(AppConsts.ONE, new PackageServiceItem { PSI_ID = AppConsts.MINUS_ONE, PSI_ServiceItemName = "Self" });
                tempSvcParentItemList.Insert(AppConsts.NONE, new PackageServiceItem { PSI_ID = AppConsts.NONE, PSI_ServiceItemName = AppConsts.COMBOBOX_ITEM_SELECT });
            }
            else
            {
                tempSvcParentItemList = new List<PackageServiceItem>();
                tempSvcParentItemList.Add(new PackageServiceItem { PSI_ID = AppConsts.NONE, PSI_ServiceItemName = AppConsts.COMBOBOX_ITEM_SELECT });
                tempSvcParentItemList.Add(new PackageServiceItem { PSI_ID = AppConsts.MINUS_ONE, PSI_ServiceItemName = "Self" });
            }
            cmbQuantityGroups.DataSource = tempSvcParentItemList;
            cmbQuantityGroups.DataBind();
        }


        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
        }

        private void ShowAddServiceItemBlock()
        {
            Presenter.LoadDropDownData();
            BindCombo(cmbServiceItemType, CurrentViewContext.ListServiceItemType);
            BindCombo(cmbParentFeeItem, CurrentViewContext.ListParentServiceItem);
            BindCombo(cmbAttributeGroup, CurrentViewContext.ListAttributeGroup);
            BindCombo(cmbGlobalFeeItem, CurrentViewContext.GlobalPackageServiceFeeItemList);

            List<lkpServiceItemPriceType> _lstPriceTypes = _presenter.GetPriceTypes(this.TenantId);
            _lstPriceTypes.Insert(AppConsts.NONE, new lkpServiceItemPriceType { SIPT_ID = AppConsts.NONE, SIPT_Name = AppConsts.COMBOBOX_ITEM_SELECT });
            BindCombo(cmbPriceType, _lstPriceTypes);

            divButtonSave.Visible = true;
            divServiceItem.Visible = true;
        }

        /// <summary>
        /// Method to clear all the controls data in its initial state.
        /// </summary>
        private void ClearControls()
        {
            txtServiceItemDescription.Text = String.Empty;
            //txtServiceItemLabel.Text = String.Empty;
            txtServiceItemName.Text = String.Empty;
            ntxtAddOccPrice.Text = String.Empty;
            cmbAttributeGroup.SelectedValue = AppConsts.MINUS_ONE.ToString();
            cmbGlobalFeeItem.SelectedValue = AppConsts.MINUS_ONE.ToString();
            cmbParentFeeItem.SelectedValue = AppConsts.MINUS_ONE.ToString();
            cmbServiceItemType.SelectedValue = AppConsts.MINUS_ONE.ToString();
            chkRequired.Checked = false;
            divServiceItem.Visible = false;
            divButtonSave.Visible = false;
            txtMinOccurrences.Text = String.Empty;
            txtMaxOccurrences.Text = String.Empty;
            txtQuantityIncluded.Text = String.Empty;
            cmbServiceItemType.SelectedValue = AppConsts.MINUS_ONE.ToString();
            cmbQuantityGroups.SelectedValue = AppConsts.ZERO;
        }

        private void SetServiceSettings()
        {
            Boolean? showdivMinOcc, showdivMaxOcc;
            showdivMinOcc = showdivMaxOcc = false;
            Entity.ApplicableServiceSetting serviceSettings = Presenter.GetServiceSettings();
            if (serviceSettings.IsNotNull())
            {
                showdivMinOcc = serviceSettings.ASSE_ShowMinOcuurence;
                showdivMaxOcc = serviceSettings.ASSE_ShowMaxOcuurence;
            }
            if ((showdivMaxOcc.HasValue && !showdivMaxOcc.Value) && (showdivMinOcc.HasValue && !showdivMinOcc.Value))
            {
                divSettings.Style.Add("display", "none");
            }
            else
            {
                if (cmbQuantityGroups.SelectedValue == "-1")
                {
                    divSettings.Style.Add("display", "block");
                    divMinOcc.Visible = (showdivMinOcc.HasValue) ? showdivMinOcc.Value : false;
                    divMaxOcc.Visible = (showdivMaxOcc.HasValue) ? showdivMaxOcc.Value : false;
                }
                else
                {
                    divSettings.Style.Add("display", "none");
                }
            }
        }
        #endregion

        #region public Methods
        #endregion

        #endregion

        #region Apply permissions

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "AddServiceItem";
                objClsFeatureAction.CustomActionLabel = "Add Service Item";
                objClsFeatureAction.ScreenName = "Manage Service Item";
                actionCollection.Add(objClsFeatureAction);
                //objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                //objClsFeatureAction.CustomActionId = "Save";
                //objClsFeatureAction.CustomActionLabel = "Save";
                //objClsFeatureAction.ScreenName = "Manage Service Item";
                //actionCollection.Add(objClsFeatureAction);
                //objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                //objClsFeatureAction.CustomActionId = "Cancel";
                //objClsFeatureAction.CustomActionLabel = "Cancel";
                //objClsFeatureAction.ScreenName = "Manage Service Item";
                //actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "DeleteColumn";
                objClsFeatureAction.ScreenName = "Manage Service Item";
                actionCollection.Add(objClsFeatureAction);
                return actionCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

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
                            {
                                if (x.FeatureAction.CustomActionId == "AddServiceItem")
                                {
                                    btnAddServiceItem.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdServiceItem.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                //else if (x.FeatureAction.CustomActionId == "Save")
                                //{
                                //    btnSave.Enabled = false;
                                //}
                                //else if (x.FeatureAction.CustomActionId == "Cancel")
                                //{
                                //    btnCancel.Enabled = false;
                                //}
                                break;
                            }
                        case AppConsts.FOUR:
                            {

                                if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdServiceItem.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "AddServiceItem")
                                {
                                    btnAddServiceItem.Visible = false;
                                }
                                //else if (x.FeatureAction.CustomActionId == "Save")
                                //{
                                //    btnSave.Visible = false;
                                //}
                                //else if (x.FeatureAction.CustomActionId == "Cancel")
                                //{
                                //    btnCancel.Visible = false;
                                //}
                                break;
                            }
                    }

                }
                    );
            }
        }
        #endregion
    }
}