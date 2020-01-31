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
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using Telerik.Web.UI;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public partial class EditServiceItemDetail : BaseUserControl, IEditServiceItemDetailView
    {
        #region Variables

        #region Private Variables

        private List<PackageServiceItem> _quantityGrps;
        private EditServiceItemDetailPresenter _presenter = new EditServiceItemDetailPresenter();
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties

        public EditServiceItemDetailPresenter Presenter
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

        public Int32 BPHM_Id
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

        String IEditServiceItemDetailView.ServiceItemName
        {
            get
            {
                return txtServiceItemName.Text.Trim();
            }
            set
            {
                txtServiceItemName.Text = value;
            }

        }

        String IEditServiceItemDetailView.ServiceItemDescription
        {
            get
            {
                return txtServiceItemDescription.Text.Trim();
            }
            set
            {
                txtServiceItemDescription.Text = value;
            }
        }

        Int32 IEditServiceItemDetailView.ServiceItemTypeId
        {
            get
            {
                if (!cmbServiceItemType.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbServiceItemType.SelectedValue);
                return AppConsts.NONE;
            }
            set
            {
                if (cmbServiceItemType.Items.Count() > 0)
                    cmbServiceItemType.SelectedValue = Convert.ToString(value);
            }
        }

        Int32? IEditServiceItemDetailView.QuantityIncluded
        {
            get
            {
                if (!txtQuantityIncluded.Text.IsNullOrEmpty())
                    return Convert.ToInt32(txtQuantityIncluded.Text);
                return null;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                    txtQuantityIncluded.Text = Convert.ToString(value);
            }
        }

        Int32? IEditServiceItemDetailView.QuantityGroupId
        {
            get
            {
                if (!cmbQuantityGroups.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbQuantityGroups.SelectedValue);
                return AppConsts.NONE;
            }
            set
            {
                if (cmbQuantityGroups.Items.Count() > 0)
                    if (value == 0)
                        cmbQuantityGroups.SelectedValue = "0";
                    else if (value == PSI_ID)
                        cmbQuantityGroups.SelectedValue = "-1";
                    else
                        cmbQuantityGroups.SelectedValue = Convert.ToString(value);
            }
        }


        Int32 IEditServiceItemDetailView.ServiceItemPriceTypeId
        {
            get
            {
                if (!cmbPriceType.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbPriceType.SelectedValue);
                return AppConsts.NONE;
            }
            set
            {
                if (cmbPriceType.Items.Count() > 0)
                    cmbPriceType.SelectedValue = Convert.ToString(value);
            }
        }
        //String IEditServiceItemDetailView.ServiceItemLabel
        //{
        //    get
        //    {
        //        return txtServiceItemLabel.Text.Trim();
        //    }
        //    set
        //    {
        //        txtServiceItemLabel.Text = value;
        //    }
        //}

        List<lkpServiceItemType> IEditServiceItemDetailView.ListServiceItemType
        {
            get
            {
                List<lkpServiceItemType> LstServiceItemType = new List<lkpServiceItemType>();
                LstServiceItemType = (List<lkpServiceItemType>)ViewState["ListServiceItemType"];
                if (LstServiceItemType.IsNotNull() && LstServiceItemType.Count > 0)
                {
                    return LstServiceItemType;
                }
                return new List<lkpServiceItemType>();
            }
            set
            {
                ViewState["ListServiceItemType"] = value;
                cmbServiceItemType.DataSource = value;
                cmbServiceItemType.DataBind();
            }
        }

        List<PackageServiceItem> IEditServiceItemDetailView.QuantityGroups
        {
            set
            {
                _quantityGrps = value;
                BindQuantityGrps(_quantityGrps);
            }
            get
            {
                return _quantityGrps;
            }
        }

        public Int32? MinOccurrences
        {
            get
            {
                return txtMinOccurrences.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMinOccurrences.Text);
            }
            set
            {
                txtMinOccurrences.Text = value.HasValue ? value.ToString() : null;
            }
        }

        public Int32? MaxOccurrences
        {
            get
            {
                return txtMaxOccurrences.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMaxOccurrences.Text);
            }
            set
            {
                txtMaxOccurrences.Text = value.HasValue ? value.ToString() : null;
            }
        }

        Int32 IEditServiceItemDetailView.BackgroundServiceId
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

        public Int32 PSI_ID
        {
            get
            {
                if (!ViewState["PSI_ID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["PSI_ID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PSI_ID"] = value;
            }
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

        Int32 IEditServiceItemDetailView.AttributeGroupId
        {
            get
            {
                if (!cmbAttributeGroup.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbAttributeGroup.SelectedValue);
                return AppConsts.NONE;
            }
            set
            {
                if (cmbAttributeGroup.Items.Count() > 0)
                    cmbAttributeGroup.SelectedValue = Convert.ToString(value);
            }
        }

        Int32 IEditServiceItemDetailView.GlobalFeeItemId
        {
            get
            {
                if (!cmbGlobalFeeItem.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbGlobalFeeItem.SelectedValue);
                /*return  GlobalFeeItemId to minus one if dropdown selected value is null or empty
                 * -[UAT-831]: WB: Whenever a new Service Item is created without any “Global Fee Item” filled in, by default the value “All County Fee” is saved in the application.*/
                return AppConsts.MINUS_ONE;
            }
            set
            {
                if (cmbGlobalFeeItem.Items.Count() > 0)
                    cmbGlobalFeeItem.SelectedValue = Convert.ToString(value);
            }
        }

        Int32? IEditServiceItemDetailView.ParentServiceItemId
        {
            get
            {
                if (!cmbGlobalFeeItem.SelectedValue.IsNullOrEmpty())
                    if (Convert.ToInt32(cmbParentFeeItem.SelectedValue) == -1)
                    {
                        return null;
                    }
                    else
                        return Convert.ToInt32(cmbParentFeeItem.SelectedValue);
                return AppConsts.NONE;
            }
            set
            {
                if (cmbParentFeeItem.Items.Count() > 0)
                    cmbParentFeeItem.SelectedValue = Convert.ToString(value);
            }
        }

        List<BkgSvcAttributeGroup> IEditServiceItemDetailView.ListAttributeGroup
        {
            set
            {
                cmbAttributeGroup.DataSource = value;
                cmbAttributeGroup.DataBind();
            }
        }
        List<PackageServiceItem> IEditServiceItemDetailView.ListParentServiceItem
        {
            set
            {
                cmbParentFeeItem.DataSource = value;
                cmbParentFeeItem.DataBind();
            }
        }

        List<Entity.PackageServiceItemFee> IEditServiceItemDetailView.GlobalPackageServiceFeeItemList
        {
            set
            {
                cmbGlobalFeeItem.DataSource = value;
                cmbGlobalFeeItem.DataBind();
            }
        }

        List<lkpServiceItemPriceType> IEditServiceItemDetailView.ServiceItemPriceTypes
        {
            set
            {
                cmbPriceType.DataSource = value;
                cmbPriceType.DataBind();
            }
        }

        Boolean IEditServiceItemDetailView.IsRequired
        {
            get
            {
                return chkRequired.Checked;
            }
            set
            {
                chkRequired.Checked = value;
            }
        }

        Boolean IEditServiceItemDetailView.IsSupplemental
        {
            get
            {
                return chkSupplement.Checked;
            }
            set
            {
                chkSupplement.Checked = value;
            }
        }

        Decimal? IEditServiceItemDetailView.AdditinalOccurencePrice
        {
            get
            {
                if (!ntxtAddOccPrice.Text.Trim().IsNullOrEmpty())
                    return Convert.ToDecimal(ntxtAddOccPrice.Text.Trim());
                return null;
            }
            set
            {
                ntxtAddOccPrice.Text = Convert.ToString(value);
            }
        }

        Int32 IEditServiceItemDetailView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Boolean IEditServiceItemDetailView.ifQuantityGrpEditable
        {
            get
            {
                if (!ViewState["ifQuantityGrpEditable"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["ifQuantityGrpEditable"]);
                return true;
            }
            set
            {
                ViewState["ifQuantityGrpEditable"] = value;
            }
        }

        public Boolean IsStateSearchRuleExists
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsStateSearchRuleExists"]);
            }
            set
            {
                ViewState["IsStateSearchRuleExists"] = value;
            }
        }

        #region Current View Context
        private IEditServiceItemDetailView CurrentViewContext
        {
            get { return this; }
        }
        #endregion

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
                    Presenter.OnViewInitialized();
                    SetServiceDetails();
                    EnableDisableControls(false);
                    //                    ShowHideCreateRuleControl();
                    ShowHideQuantityIncludedPanel();
                    ApplyActionLevelPermission(ActionCollection, "Manage Service Item");
                }
                ShowHideCreateRuleControl();

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

        protected void cmbAttributeGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                List<PackageServiceItem> _tempSvcParentItemList = new List<PackageServiceItem>();
                if (!String.IsNullOrEmpty(cmbAttributeGroup.SelectedValue) && cmbAttributeGroup.SelectedValue != AppConsts.ZERO)
                {
                    Int32 _selectAttrGrpId = Convert.ToInt32(cmbAttributeGroup.SelectedValue);
                    Presenter.GetQuantityGroups(_selectAttrGrpId, this.BPHM_Id, this.TenantId);
                    BindQuantityGrps(CurrentViewContext.QuantityGroups);
                }
                ShowHideQuantityIncludedPanel();
                divSettings.Style.Add("display", "none");
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

        protected void cmbQuantityGroups_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ShowHideQuantityIncludedPanel();
                SetServiceDetails();
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

        #region Button events

        /// <summary>
        /// Event to save data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.AdditinalOccurencePrice.IsNullOrEmpty() && CurrentViewContext.AdditinalOccurencePrice >= AppConsts.NONE)
                {
                    if (CurrentViewContext.ServiceItemPriceTypeId == 0)
                    {
                        (this.Page as BaseWebPage).ShowInfoMessage("Please select additional occurence price type.");
                        return;
                    }
                }
                if (Presenter.UpdateServiceItemData())
                {

                    if (CurrentViewContext.IsSupplemental)
                    {
                        Page.Form.FindControl("MainContent").FindControl("dvCustomForms").Visible = true;
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), Guid.NewGuid().ToString(), "RefreshPage();", true);
                    }
                    else
                        Page.Form.FindControl("MainContent").FindControl("dvCustomForms").Visible = false;

                    EnableDisableControls(false);
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
                Presenter.OnViewInitialized();
                EnableDisableControls(false);
                SetServiceDetails();
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

        /// <summary>
        /// Event to cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarEdit_Click(object sender, EventArgs e)
        {
            try
            {
                EnableDisableControls(true);
                //ShowHideCreateRuleControl();
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
        protected void btnCreateRule_Click(object sender, EventArgs e)
        {
            try
            {
                String errorMsg = Presenter.CreateAutomaticSrchRule();
                if (errorMsg == String.Empty)
                {
                    if (CurrentViewContext.IsSupplemental)
                    {
                        Page.Form.FindControl("MainContent").FindControl("dvCustomForms").Visible = true;
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), Guid.NewGuid().ToString(), "RefreshPage();", true);
                    }
                    else
                        Page.Form.FindControl("MainContent").FindControl("dvCustomForms").Visible = false;

                    EnableDisableControls(false);
                    //Code added corresponding to UAT - 917 : When user clicks on “Create Rule” button then added ruleset doesn’t appear in “Rulesets” grid present on right pane in institution hierarchy screen
                    RadGrid grdRuleSet = (Page.Form.FindControl("MainContent").FindControl("ucRuleSetList").FindControl("grdRuleSet")) as RadGrid;
                    if (grdRuleSet.IsNotNull())
                    {
                        grdRuleSet.Rebind();
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage("Rule created sucessfully.");
                }
                else
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(errorMsg);
                    //ShowHideCreateRuleControl();
                }
                ShowHideCreateRuleControl();
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
        /// Method to enable\disabled controls.
        /// </summary>
        /// <param name="isEnabled">isEnabled</param>
        private void EnableDisableControls(Boolean isEnabled)
        {
            //Enable\Disable controls.
            txtServiceItemDescription.Enabled = isEnabled;
            //txtServiceItemLabel.Enabled = isEnabled;
            txtServiceItemName.Enabled = isEnabled;
            ntxtAddOccPrice.Enabled = isEnabled;
            if (isEnabled == false)
                cmbAttributeGroup.Enabled = isEnabled;
            else
                cmbAttributeGroup.Enabled = Presenter.ifAttribteGrpEditable();
            cmbGlobalFeeItem.Enabled = isEnabled;
            cmbParentFeeItem.Enabled = isEnabled;
            cmbServiceItemType.Enabled = isEnabled;
            chkRequired.Enabled = isEnabled;
            chkSupplement.Enabled = isEnabled;
            cmbPriceType.Enabled = isEnabled;
            txtQuantityIncluded.Enabled = isEnabled;

            if (isEnabled == false)
                cmbQuantityGroups.Enabled = isEnabled;
            else
                cmbQuantityGroups.Enabled = CurrentViewContext.ifQuantityGrpEditable;
            txtMinOccurrences.Enabled = isEnabled;
            txtMaxOccurrences.Enabled = isEnabled;

            //Enable\Disable buttons.
            btnSave.Visible = isEnabled;
            btnCancel.Visible = isEnabled;
            btnEdit.Visible = !isEnabled;
        }

        /// <summary>
        /// Method to clear all the controls data in its initial state.
        /// </summary>
        private void ClearControls()
        {
            txtServiceItemDescription.Text = String.Empty;
            // txtServiceItemLabel.Text = String.Empty;
            txtServiceItemName.Text = String.Empty;
            ntxtAddOccPrice.Text = String.Empty;
            cmbAttributeGroup.SelectedValue = AppConsts.MINUS_ONE.ToString();
            cmbGlobalFeeItem.SelectedValue = AppConsts.MINUS_ONE.ToString();
            cmbParentFeeItem.SelectedValue = AppConsts.MINUS_ONE.ToString();
            cmbServiceItemType.SelectedValue = AppConsts.MINUS_ONE.ToString();
            chkRequired.Checked = false;
            chkSupplement.Checked = false;
            cmbPriceType.SelectedValue = AppConsts.ZERO;
            BindQuantityGrps();
        }

        /// <summary>
        /// Manage the Quantity group dropdown
        /// </summary>
        /// <param name="tempSvcParentItemList"></param>
        /// <param name="selectAttrGrpId"></param>
        private void BindQuantityGrps(List<PackageServiceItem> tempSvcParentItemList = null)
        {
            if (!tempSvcParentItemList.IsNullOrEmpty())
            {
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

        private void SetServiceDetails()
        {
            Boolean? showdivMinOcc, showdivMaxOcc;
            showdivMinOcc = showdivMaxOcc = false;

            Entity.ApplicableServiceSetting serviceSettings = Presenter.GetServiceSettings();
            if (serviceSettings.IsNotNull())
            {
                showdivMinOcc = serviceSettings.ASSE_ShowMinOcuurence;
                showdivMaxOcc = serviceSettings.ASSE_ShowMaxOcuurence;
            }
            if ((showdivMinOcc.HasValue && !showdivMinOcc.Value) && (showdivMaxOcc.HasValue && !showdivMaxOcc.Value))
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

        #region Public Methods
        #endregion
        #endregion

        #region Apply permissions

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Edit";
                objClsFeatureAction.CustomActionLabel = "Edit Service Item";//Edit;
                objClsFeatureAction.ScreenName = "Manage Service Item"; //"Update Service Item";
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
                                if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    btnEdit.Enabled = false;
                                    btnCreateRule.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {

                                if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    btnEdit.Visible = false;
                                    btnCreateRule.Visible = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        #endregion
        }
        private void ShowHideCreateRuleControl()
        {
            Presenter.IsStateSearchRuleExists();
            if (!CurrentViewContext.IsStateSearchRuleExists)
            {
                if (CurrentViewContext.ListServiceItemType.IsNotNull() && CurrentViewContext.ListServiceItemType.Count > 0)
                {
                    List<lkpServiceItemType> lstServiceItemtype = CurrentViewContext.ListServiceItemType.Where(x => x.SIT_Code.Equals(ServiceItemType.Statesearch.GetStringValue()) || x.SIT_Code.Equals(ServiceItemType.CountySearch.GetStringValue())).ToList();
                    foreach (var item in lstServiceItemtype)
                    {
                        if (Convert.ToInt32(cmbServiceItemType.SelectedValue) == item.SIT_ID)
                        {
                            btnCreateRule.Visible = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                btnCreateRule.Visible = false;
            }
        }

    }
}