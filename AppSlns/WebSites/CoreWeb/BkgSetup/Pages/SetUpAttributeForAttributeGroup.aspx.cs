using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class SetUpAttributeForAttributeGroup : BaseWebPage, ISetUpAttributeForAttributeGroupView
    {

        #region Variables

        #region Private variables
        private SetUpAttributeForAttributeGroupPresenter _presenter = new SetUpAttributeForAttributeGroupPresenter();
        #endregion

        #region Public Variables

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
                    if (!Request.QueryString["tenantId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                        ApplyActionLevelPermission(ActionCollection, "Manage Package Service SetUp");
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

        #region Grid Related Events
        protected void grdMappedAttribute_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetMappedAttributeWithGroup();
                grdMappedAttribute.DataSource = CurrentViewContext.MappedAttributeList;
                if (CurrentViewContext.MappedAttributeList.Count > 0 && CurrentViewContext.MappedAttributeList.FirstOrDefault().IsNotNull())
                {
                    String attributeGroupCode = CurrentViewContext.MappedAttributeList.FirstOrDefault().AttributeGroupCode;
                    grdMappedAttribute.ClientSettings.AllowAutoScrollOnDragDrop = IsDragDropSequenceOrder(attributeGroupCode);
                    grdMappedAttribute.ClientSettings.AllowRowsDragDrop = IsDragDropSequenceOrder(attributeGroupCode);
                    grdMappedAttribute.MasterTableView.Columns.FindByUniqueName("DisplayOrder").Visible = IsDragDropSequenceOrder(attributeGroupCode);
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

        protected void grdMappedAttribute_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                //CurrentViewContext.BkgPackageSvcId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BkgPackageSvcId"));
                Int32 bkgPackageSvcAttributeMappingId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BkgPackageSvcAttributeMappingId"));
                if (Presenter.DeletedBkgSvcAttributeMapping(bkgPackageSvcAttributeMappingId))
                {
                    base.ShowSuccessMessage("Attribute mapping deleted successfully.");
                    grdMappedAttribute.Rebind();
                    Presenter.GetAllAttributeList();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
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

        protected void grdMappedAttribute_RowDrop(object sender, GridDragDropEventArgs e)
        {
            try
            {

                if (CurrentViewContext.MappedAttributeList.IsNull())
                    Presenter.GetAttributesWithGroup();
                if (String.IsNullOrEmpty(e.HtmlElement))
                {

                    if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdMappedAttribute.ClientID)
                    {
                        //reorder items in grid
                        Int32 destAttributeGroupMappingId = Convert.ToInt32(e.DestDataItem.GetDataKeyValue("BkgAttributeGroupMappingId"));
                        AttributeSetupContract selectedOrderClientStatus = CurrentViewContext.MappedAttributeList.Where(cond => cond.BkgAttributeGroupMappingId == destAttributeGroupMappingId).FirstOrDefault();
                        Int32? destinationIndex = selectedOrderClientStatus.DisplayOrder;
                        IList<AttributeSetupContract> statusToMove = new List<AttributeSetupContract>();
                        IList<AttributeSetupContract> shiftedStatus = null;
                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            Int32 draggedAttrGroupMappingId = Convert.ToInt32(draggedItem.GetDataKeyValue("BkgAttributeGroupMappingId"));
                            AttributeSetupContract tmpAttributeSetupContractList = CurrentViewContext.MappedAttributeList.Where(cond => cond.BkgAttributeGroupMappingId == draggedAttrGroupMappingId).FirstOrDefault();
                            if (tmpAttributeSetupContractList != null)
                                statusToMove.Add(tmpAttributeSetupContractList);
                        }
                        AttributeSetupContract lastAttributeToMove = statusToMove.OrderByDescending(i => i.DisplayOrder).FirstOrDefault();
                        Int32? sourceIndex = lastAttributeToMove.DisplayOrder;
                        if (sourceIndex > destinationIndex)
                        {
                            shiftedStatus = CurrentViewContext.MappedAttributeList.Where(obj => obj.DisplayOrder >= destinationIndex && obj.DisplayOrder < sourceIndex).ToList();
                            if (shiftedStatus.IsNotNull())
                                statusToMove.AddRange(shiftedStatus);
                        }
                        else if (sourceIndex < destinationIndex)
                        {
                            shiftedStatus = CurrentViewContext.MappedAttributeList.Where(obj => obj.DisplayOrder <= destinationIndex && obj.DisplayOrder > sourceIndex).ToList();
                            if (shiftedStatus.IsNotNull())
                                shiftedStatus.AddRange(statusToMove);
                            statusToMove = shiftedStatus;
                            destinationIndex = sourceIndex;
                        }
                        // Update Sequence
                        Presenter.UpdateDisplaySequence(statusToMove, destinationIndex);
                        grdMappedAttribute.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
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
        /// Event to add attribute 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarAddAttribute_Click(object sender, EventArgs e)
        {
            try
            {
                ShowAttributeBlock();
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
                if (cmbChkAttribute.SelectedValue == AppConsts.ZERO)
                {
                    ServiceAttributeContract serviceAttributeContract = new ServiceAttributeContract();
                    serviceAttributeContract.Name = txtAttributeName.Text;
                    serviceAttributeContract.AttributeLabel = txtAttributeLabel.Text;
                    serviceAttributeContract.Description = txtAttributeDescription.Text;
                    serviceAttributeContract.IsActive = chkActive.Checked;
                    //serviceAttributeContract.IsRequired = chkRequired.Checked;
                    serviceAttributeContract.ServiceAttributeDatatypeID = Convert.ToInt32(cmbDataType.SelectedValue);
                    String errorMessage = IsmappingOfThisTypeAllowed(cmbDataType.SelectedItem.Text);
                    if (errorMessage != "")
                    {
                        base.ShowErrorMessage(errorMessage);
                        return;

                    }

                    if (!IsValidOptionFormat(txtOptOptions.Text))
                    {
                        base.ShowErrorMessage("Please enter valid options format i.e. Positive=1,Negative=2.");
                        return;
                    }

                    serviceAttributeContract.lstClientServiceAttributeOption = GetServiceAttributeOption(txtOptOptions.Text);
                    if (String.IsNullOrEmpty(ntxtMaxChars.Text))
                        serviceAttributeContract.MaximumCharacters = null;
                    else
                        serviceAttributeContract.MaximumCharacters = Convert.ToInt32(ntxtMaxChars.Text);
                    if (String.IsNullOrEmpty(nTxtMinLength.Text))
                        serviceAttributeContract.MinimumCharacters = null;
                    else
                        serviceAttributeContract.MinimumCharacters = Convert.ToInt32(nTxtMinLength.Text);
                    if (String.IsNullOrEmpty(nTxtMaxIntegerValue.Text))
                        serviceAttributeContract.MaximumNumericvalue = null;
                    else
                        serviceAttributeContract.MaximumNumericvalue = Convert.ToInt32(nTxtMaxIntegerValue.Text);
                    if (String.IsNullOrEmpty(nTxtMinimunIntegerValue.Text))
                        serviceAttributeContract.MinimumNumericvalue = null;
                    else
                        serviceAttributeContract.MinimumNumericvalue = Convert.ToInt32(nTxtMinimunIntegerValue.Text);
                    if (dpkrMaxDateValue.SelectedDate == null)
                        serviceAttributeContract.MaximumDatevalue = null;
                    else
                        serviceAttributeContract.MaximumDatevalue = dpkrMaxDateValue.SelectedDate;
                    if (dpkrMinDateValue.SelectedDate == null)
                        serviceAttributeContract.MinimumDatevalue = null;
                    else
                        serviceAttributeContract.MinimumDatevalue = dpkrMinDateValue.SelectedDate;

                    serviceAttributeContract.ModifiedByID = CurrentViewContext.CurrentLoggedInUserId;
                    serviceAttributeContract.ModifiedOn = DateTime.Now;
                    serviceAttributeContract.TenantID = CurrentViewContext.TenantId;
               
                   // serviceAttributeContract.IsHiddenFromUI = chkIsHiddenFromUI.Checked;
                    if (Presenter.SaveAttributeAndMapping(serviceAttributeContract.TranslateToClientEntity()))
                    {
                        base.ShowSuccessMessage("Attribute mapping saved successfully.");
                        divAddAttribute.Visible = false;
                        divSaveButton.Visible = false;
                        ClearControls(true);
                        chkIsDisplay.Checked = false;
                        chkRequired.Checked = false;
                        ShowHideContentArea(String.Empty);
                        grdMappedAttribute.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    }

                }
                else
                {
                    if (Presenter.SaveExistingAttributeMapping())
                    {
                        base.ShowSuccessMessage("Attribute mapping saved successfully.");
                        divAddAttribute.Visible = false;
                        divSaveButton.Visible = false;
                        ClearControls(true);
                        chkIsDisplay.Checked = false;
                        chkRequired.Checked = false;
                        ShowHideContentArea(String.Empty);
                        grdMappedAttribute.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    else
                    {
                        base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
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

        public String IsmappingOfThisTypeAllowed(String attributeType)
        {
            return Presenter.IsmappingOfThisTypeAllowed(attributeType);
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
                divAddAttribute.Visible = false;
                divSaveButton.Visible = false;
                ClearControls(true);
                ShowHideContentArea(String.Empty);
                chkIsDisplay.Checked = false;
                chkRequired.Checked = false;
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
                ShowHideContentArea(cmbDataType.SelectedItem.Text, true);
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

        protected void cmbChkAttribute_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmbChkAttribute.SelectedValue == AppConsts.ZERO)
                    divAddNewAttribute.Visible = true;
                else
                {
                    ClearControls(true);
                    divAddNewAttribute.Visible = false;
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

        #region Properties

        #region Public Properties
        public SetUpAttributeForAttributeGroupPresenter Presenter
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

        Int32 ISetUpAttributeForAttributeGroupView.TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 ISetUpAttributeForAttributeGroupView.BackgroundServiceId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "SVC"));
            }
        }
        Int32 ISetUpAttributeForAttributeGroupView.BackgroundServiceGroupId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "SVCG"));
            }
        }

        Int32 ISetUpAttributeForAttributeGroupView.BackgroundPackageId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "PKG"));
            }

        }

        Int32 ISetUpAttributeForAttributeGroupView.BkgPackageSvcId
        {
            get
            {
                return (Int32)(ViewState["BkgPackageSvcId"]);
            }
            set
            {
                ViewState["BkgPackageSvcId"] = value;
            }
        }

        Int32 ISetUpAttributeForAttributeGroupView.AttributeGroupId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "ATTG"));
            }

        }

        Boolean ISetUpAttributeForAttributeGroupView.IsServiceSystemDefined
        {
            set
            {
                DisableServiceControls(value);
            }
        }

        Int32 ISetUpAttributeForAttributeGroupView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        String ISetUpAttributeForAttributeGroupView.ErrorMessage { get; set; }

        String ISetUpAttributeForAttributeGroupView.SuccessMessage { get; set; }

        String ISetUpAttributeForAttributeGroupView.InfoMessage { get; set; }

        List<lkpSvcAttributeDataType> ISetUpAttributeForAttributeGroupView.listAttributeDataType
        {
            set
            {
                cmbDataType.DataSource = value;
                cmbDataType.DataBind();
            }
        }

        List<AttributeDataSecurityClient> ISetUpAttributeForAttributeGroupView.AttributeList
        {
            set
            {
                cmbChkAttribute.DataSource = value;
                cmbChkAttribute.DataBind();
                if (cmbChkAttribute.Items.Count >= 10)
                {
                    cmbChkAttribute.Height = Unit.Pixel(200);
                }
            }
        }
        List<Int32> ISetUpAttributeForAttributeGroupView.MappedAttributeIds
        {
            get
            {
                return (List<Int32>)(ViewState["MappedAttributeIds"]);
            }
            set
            {
                ViewState["MappedAttributeIds"] = value;
            }
        }

        Int32 ISetUpAttributeForAttributeGroupView.SelectedAttributeId
        {
            get
            {
                if (!cmbChkAttribute.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbChkAttribute.SelectedValue);
                return 0;

            }
            set
            {
                for (Int32 i = 0; i < cmbChkAttribute.Items.Count; i++)
                {
                    cmbChkAttribute.SelectedValue = Convert.ToString(value);
                }
            }
        }
        List<AttributeSetupContract> ISetUpAttributeForAttributeGroupView.MappedAttributeList
        {
            get;
            set;
        }

        Boolean ISetUpAttributeForAttributeGroupView.IsRequired
        {
            get
            {
                return chkRequired.Checked;
            }
        }

        Boolean ISetUpAttributeForAttributeGroupView.IsDisplay
        {
            get
            {
                return chkIsDisplay.Checked;
            }
        }
        Boolean ISetUpAttributeForAttributeGroupView.IsHiddenFromUI
        {
            get
            {
                return chkIsHiddenFromUI.Checked;
            }
        }

        public String NodeId
        {
            get
            {
                if (!Request.QueryString["nodeId"].IsNullOrEmpty())
                {
                    return Request.QueryString["nodeId"];
                }
                return String.Empty;
            }

        }

        #region Current View Context
        private ISetUpAttributeForAttributeGroupView CurrentViewContext
        {
            get { return this; }
        }
        #endregion

        #region Private Properties

        #endregion

        #endregion
        #endregion

        #region Methods

        #region Private Methods
        private void DisableServiceControls(Boolean isServiceSystemdefined)
        {
        }

        /// <summary>
        /// Method to show Add new block.
        /// </summary>
        private void ShowAttributeBlock()
        {
            divAddAttribute.Visible = true;
            divAddNewAttribute.Visible = true;
            divSaveButton.Visible = true;
            Presenter.GetAttributeDataType();
            Presenter.GetBkgPackageSvcId();
            Presenter.GetAllAttributeList();
        }

        /// <summary>
        /// Method to check the Valid option format
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private Boolean IsValidOptionFormat(String options)
        {
            if (!String.IsNullOrEmpty(options))
            {
                string[] arrayOfOptions = options.Split(',');
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

        /// <summary>
        /// Method that convert the options in entity.
        /// </summary>
        /// <param name="options">options</param>
        /// <returns></returns>
        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption> GetServiceAttributeOption(String options)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption> lstServiceAttributeOption = null;
            if (String.IsNullOrEmpty(options))
                return lstServiceAttributeOption;

            string[] arrayOfOptions = options.Split(',');
            if (arrayOfOptions.Length > 0)
            {
                for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                {
                    string[] option = arrayOfOptions[counter].Split('=');
                    if (option.Length.Equals(2))
                    {
                        if (lstServiceAttributeOption == null)
                            lstServiceAttributeOption = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption>();

                        lstServiceAttributeOption.Add(new BkgSvcAttributeOption()
                        {
                            EBSAO_OptionText = option[0],
                            EBSAO_OptionValue = option[1],
                            EBSAO_CreatedByID = CurrentViewContext.CurrentLoggedInUserId,
                            EBSAO_CreatedOn = DateTime.Now,
                            EBSAO_IsActive = true,
                            EBSAO_IsDeleted = false

                        });
                    }
                }
            }
            return lstServiceAttributeOption;
        }

        /// <summary>
        /// Method to show the content area on selection of data type.
        /// </summary>
        /// <param name="complianceAttributeDatatype">complianceAttributeDatatype</param>
        /// <param name="clearData">clearData</param>
        private void ShowHideContentArea(string complianceAttributeDatatype, bool clearData = false)
        {
            if (clearData)
            {
                ClearControls(false);
            }
            divOption.Visible = false;
            divCharacters.Visible = false;
            divDateType.Visible = false;
            divIntegerType.Visible = false;

            if (divOption != null && complianceAttributeDatatype.Equals("Option"))
            {
                divOption.Visible = true;
            }

            if (divCharacters != null && complianceAttributeDatatype.Equals("Text"))
            {
                divCharacters.Visible = true;
            }

            if (divDateType != null && complianceAttributeDatatype.Equals("Date"))
            {
                divDateType.Visible = true;
            }
            if (divIntegerType != null && complianceAttributeDatatype.Equals("Numeric"))
            {
                divIntegerType.Visible = true;
            }
        }

        /// <summary>
        /// Method that clear all the controls.
        /// </summary>
        private void ClearControls(Boolean IsClearAllControl)
        {
            if (IsClearAllControl)
            {
                txtAttributeName.Text = String.Empty;
                txtAttributeDescription.Text = String.Empty;
                txtAttributeLabel.Text = String.Empty;
                chkActive.Checked = false;
                cmbDataType.SelectedValue = AppConsts.ZERO;
            }
            txtOptOptions.Text = string.Empty;
            ntxtMaxChars.Text = string.Empty;
            nTxtMinLength.Text = string.Empty;
            nTxtMaxIntegerValue.Text = string.Empty;
            nTxtMinimunIntegerValue.Text = string.Empty;
            dpkrMaxDateValue.SelectedDate = null;
            dpkrMinDateValue.SelectedDate = null;
        }

        private Boolean IsDragDropSequenceOrder(String attributeGroupCode)
        {
            if (attributeGroupCode.ToUpper() == AppConsts.MVR_ATTRIBUTE_GROUP_CODE || attributeGroupCode.ToUpper() == AppConsts.RESIDENTIAL_HISTORY_ATTRIBUTE_GROUP_CODE || attributeGroupCode.ToUpper() == AppConsts.PERSONAL_INFORMATION_ATTRIBUTE_GROUP_CODE || attributeGroupCode.ToUpper() == AppConsts.PERSONAL_ALIAS_ATTRIBUTE_GROUP_CODE)
                return false;
            return true;
        }
        #endregion

        #region Public Methods

        #endregion

        #endregion

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
                            {
                                if (x.FeatureAction.CustomActionId == "Add Attribute")
                                {
                                    btnEdit.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete Attribute")
                                {
                                    grdMappedAttribute.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Add Attribute")
                                {
                                    btnEdit.Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete Attribute")
                                {
                                    grdMappedAttribute.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                });
            }
        }

        #endregion


    }
}