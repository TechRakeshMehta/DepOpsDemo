﻿using System;
using Microsoft.Practices.ObjectBuilder;
using CoreWeb.Shell;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Xml;
using System.Linq;
using Telerik.Web.UI;
using INTSOF.SharedObjects;
using CoreWeb.IntsofSecurityModel;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ShotSeriesRuleInfo : BaseWebPage, IShotSeriesRuleInfoView
    {
        #region Variable

        #region Private Variables
        private ShotSeriesRuleInfoPresenter _presenter = new ShotSeriesRuleInfoPresenter();
        Int32 tenantid = 0;
        private ComplianceDataStore _complinceDataStore;
        private List<lkpRuleObjectMappingType> _listRuleObjectMappingType;
        private List<lkpConstantType> _constantTypeList;
        private String _settingXml = String.Empty;
        private Boolean _isScheduleActionRecordInserted = false;
        private List<ItemSeriesItem> _listItemSeriesItem;
        #endregion

        #region Public variables

        #endregion
        #endregion

        #region Properties

        public ShotSeriesRuleInfoPresenter Presenter
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

        public int TenantId
        {
            get
            {
                if (tenantid == 0)
                {
                    //tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantid;
            }
            set { tenantid = value; }
        }

        public int CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public int SelectedRuleTemplateId
        {
            get;
            set;

        }

        public List<RuleTemplate> lstRuleTemplates
        {
            get;
            set;
        }

        public RuleTemplate RuleTemplateDetails
        {
            get;
            set;

        }

        public IShotSeriesRuleInfoView CurrentViewContext
        {
            get { return this; }
        }

        public RuleMapping RuleMapping
        {
            get;
            set;

        }

        public int RuleMappingId
        {
            get
            {
                return Convert.ToInt32(ViewState["RuleMappingId"]);
            }
            set
            {
                ViewState["RuleMappingId"] = value;
            }

        }

        public Int32 CategoryId
        {
            get
            {
                if (ViewState["CategoryId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["CategoryId"]);
            }
            set
            {
                ViewState["CategoryId"] = value;
            }
        }

        public int RuleSetId
        {
            get
            {
                if (ViewState["RuleSetId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["RuleSetId"]);
            }
            set
            {
                ViewState["RuleSetId"] = value;
            }
        }

        public Int32 ObjectCount
        {
            get
            {

                if (hdnObjectCount == null || String.IsNullOrEmpty(hdnObjectCount.Value))
                    return 0;
                return Convert.ToInt32(hdnObjectCount.Value);
            }
            set
            {
                hdnObjectCount.Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Boolean IsVersionUpdate
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsVersionUpdate"]);
            }
            set
            {
                ViewState["IsVersionUpdate"] = value;
            }
        }

        public Int32 PreviousRuleMappingId
        {
            get
            {
                return Convert.ToInt32(ViewState["PreviousRuleMappingId"]);
            }
            set
            {
                ViewState["PreviousRuleMappingId"] = value;
            }

        }

        public Int32? FirstVersionRuleId
        {
            get
            {
                if (ViewState["FirstVersionRuleId"] == null)
                    return null;
                return Convert.ToInt32(ViewState["FirstVersionRuleId"]);
            }
            set
            {
                ViewState["FirstVersionRuleId"] = value;
            }
        }

        public Boolean IfAnySubscriptionExist
        {
            get
            {
                return Convert.ToBoolean(ViewState["IfAnySubscriptionExist"]);
            }
            set
            {
                ViewState["IfAnySubscriptionExist"] = value;
            }
        }

        ComplianceDataStore complinceDataStore
        {
            get
            {
                if (_complinceDataStore.IsNull())
                {
                    _complinceDataStore = new ComplianceDataStore();
                }
                return _complinceDataStore;
            }
            set
            {
                _complinceDataStore = value;
            }
        }

        List<lkpRuleObjectMappingType> listRuleObjectMappingType
        {
            get
            {
                if (_listRuleObjectMappingType == null)
                {
                    _listRuleObjectMappingType = Presenter.GetRuleObjectMappingType();
                }
                return _listRuleObjectMappingType;
            }
            set
            {
                _listRuleObjectMappingType = value;
            }
        }

        List<lkpConstantType> ConstantTypeList
        {
            get
            {
                if (_constantTypeList == null)
                {
                    _constantTypeList = Presenter.getConstantType();
                }
                return _constantTypeList;
            }
            set
            {
                _constantTypeList = value;
            }
        }

        public RuleSynchronisationData RuleSynchronisationData
        {
            get
            {
                if (!(ViewState["RuleSynchronisationData"] is RuleSynchronisationData))
                {
                    ViewState["RuleSynchronisationData"] = new RuleSynchronisationData();
                }
                return (RuleSynchronisationData)ViewState["RuleSynchronisationData"];
            }
            set
            {
                ViewState["RuleSynchronisationData"] = value;
            }
        }

        public String SettingXml
        {
            get
            {
                return _settingXml;
            }
            set { _settingXml = value; }
        }

        public Boolean IsScheduleActionRecordInserted
        {
            get
            {
                return _isScheduleActionRecordInserted;
            }
            set { _isScheduleActionRecordInserted = value; }
        }

        public String ErrMsg
        {
            get;
            set;
        }

        List<ItemSeriesItem> listItemSeriesItem
        {
            get
            {
                if (_listItemSeriesItem.IsNullOrEmpty())
                {
                    _listItemSeriesItem = Presenter.GetItemSeriesItemList();
                }
                return _listItemSeriesItem;
            }
            set
            {
                _listItemSeriesItem = value;
            }
        }

        public Int32 SeriesId
        {
            get
            {
                return Convert.ToInt32(ViewState["SeriesId"]);
            }
            set
            {
                ViewState["SeriesId"] = value;
            }
        }

        public String SelectedObjectTypeCode
        {
            get
            {
                return ddlObjectType.SelectedValue;
            }
            set
            {
                ddlObjectType.SelectedValue = value.ToString();
            }

        }

        public List<Int32> SelectedObjectIds
        {
            get;
            set;
        }
        #endregion


        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["Id"] != null)
                {
                    CurrentViewContext.RuleMappingId = Convert.ToInt32(Request.QueryString["Id"]);
                }

                if (Request.QueryString["SelectedTenantId"] != null)
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);

                if (Request.QueryString["SeriesId"] != null)
                {
                    CurrentViewContext.SeriesId = Convert.ToInt32(Request.QueryString["SeriesId"]);
                }

                if (Request.QueryString["CategoryId"] != null)
                {
                    CategoryId = Convert.ToInt32(Request.QueryString["CategoryId"]);
                }

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindRuleInfo();
                    ResetButtons(true);
                    SetFormMode(false);
                }
                Presenter.OnViewLoaded();
                if (hdnIsCancelRequest.Value != "")
                {
                    if (Convert.ToBoolean(hdnIsCancelRequest.Value))
                    {
                        // BindRuleInfo();
                    }
                }
                else
                {
                    LoadExpressionObjects();
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

        protected void fsucCmdBarRule_CancelClick(object sender, EventArgs e)
        {
            try
            {
                BindRuleInfo();
                ResetButtons(true);
                SetFormMode(false);
                LoadExpressionObjects();
                hdnIsCancelRequest.Value = "";
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

        protected void fsucCmdBarRule_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                lblSuccess.Visible = false;
                BindRuleInfo();
                ResetButtons(false);
                SetFormMode(true);
                CurrentViewContext.IsVersionUpdate = false;
                fsucCmdBarRule.SaveButton.ValidationGroup = "grpFormSubmit";
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

        protected void fsucCmdBarRule_SaveClick(object sender, EventArgs e)
        {
            try
            {
                RuleMapping mappingToBeSaved = GetRuleMapping();
                String errorMsg = String.Empty;
                foreach (RuleMappingDetail ruleMapping in mappingToBeSaved.RuleMappingDetails)
                {
                    if (!ruleMapping.IfExpressionIsvalid)
                    {
                        errorMsg += String.Format(" Please specify mapping for  \"" + ruleMapping.RLMD_PlaceHolderName + "\".\n");
                    }
                    else if (ruleMapping.RLMD_ConstantValue != String.Empty && ruleMapping.RLMD_ConstantValue != null)
                    {
                        errorMsg += constantInput(ruleMapping);
                    }
                }
                if (errorMsg != String.Empty)
                {
                    if (errorMsg.IndexOf("\n") == errorMsg.LastIndexOf("\n"))
                    {
                        txtValidationResult.Text = String.Format("Error: {0}", errorMsg);
                    }
                    else
                    {
                        txtValidationResult.Text = String.Format("Error: \n{0}", errorMsg);
                    }
                }
                else
                {

                    RuleProcessingResult xmlOutputString = validateRule(mappingToBeSaved.RuleMappingDetails.ToList());
                    if (xmlOutputString.Status == 1)
                    {
                        txtValidationResult.Text = String.Format("Error: {0}", xmlOutputString.ErrorMessage);
                        btnValidate.Text = "Re-validate Rule";
                    }
                    else
                    {
                        //Added new  logic for editablity of rule 
                        mappingToBeSaved.RLM_IsCurrent = true;
                        mappingToBeSaved.RLM_UIExpression = xmlOutputString.UIExpressionLabel;

                        SelectedObjectIds = new List<Int32>();
                        if (SelectedObjectTypeCode == ObjectType.Series_Item.GetStringValue())
                        {
                            foreach (RadComboBoxItem item in cmbSeriesItems.CheckedItems)
                            {
                                SelectedObjectIds.Add(Convert.ToInt32(item.Value));
                            }
                        }
                        else
                        {
                            SelectedObjectIds.Add(SeriesId);
                        }

                        Presenter.UpdateRuleMapping(mappingToBeSaved);
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                        BindRuleInfo();
                        ResetButtons(true);
                        SetFormMode(false);
                        //lblSuccess.Visible = true;
                        base.ShowSuccessMessage("Rule updated successfully.");

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

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                List<RuleMappingDetail> ruleMappingList = GetRuleMappingDetail();
                String errorMsg = String.Empty;
                foreach (RuleMappingDetail ruleMapping in ruleMappingList)
                {
                    if (!ruleMapping.IfExpressionIsvalid)
                    {
                        errorMsg += String.Format(" Please specify mapping for  \"" + ruleMapping.RLMD_PlaceHolderName + "\".\n");
                    }
                    else if (ruleMapping.RLMD_ConstantValue != String.Empty && ruleMapping.RLMD_ConstantValue != null)
                    {
                        errorMsg += constantInput(ruleMapping);
                    }
                }
                if (errorMsg != String.Empty)
                {
                    if (errorMsg.IndexOf("\n") == errorMsg.LastIndexOf("\n"))
                    {
                        txtValidationResult.Text = String.Format("Error: {0}", errorMsg);
                    }
                    else
                    {
                        txtValidationResult.Text = String.Format("Error: \n{0}", errorMsg);
                    }
                }
                else
                {

                    RuleProcessingResult xmlOutputString = validateRule(ruleMappingList);
                    if (xmlOutputString.Status == 0)
                    {
                        txtValidationResult.Text = String.Format("Success: {0}", xmlOutputString.UIExpressionLabel);
                    }
                    else
                    {
                        txtValidationResult.Text = String.Format("Error: {0}", xmlOutputString.ErrorMessage);

                        btnValidate.Text = "Re-validate Rule";
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

        protected void fsucCmdBarRule_ClearClick(object sender, EventArgs e)
        {
            try
            {
                lblSuccess.Visible = false;
                CurrentViewContext.IsVersionUpdate = true;
                BindRuleInfo();
                ResetButtons(false);
                SetFormMode(true);
                fsucCmdBarRule.SaveButton.ValidationGroup = "grpFormSubmit";
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

        protected void cmbMasterTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlExpressionObjects.Controls.Clear();
                if (cmbMasterTemplates.SelectedValue == "0")
                {
                    litExpression.Text = string.Empty;
                    lblAction.Text = String.Empty;
                    return;
                }
                CurrentViewContext.SelectedRuleTemplateId = Convert.ToInt32(cmbMasterTemplates.SelectedValue);
                Presenter.GetRuleTemplateDetails();
                ObjectCount = CurrentViewContext.RuleTemplateDetails.RLT_ObjectCount;
                lblAction.Text = CurrentViewContext.RuleTemplateDetails.lkpRuleActionType.ACT_Description;

                litExpression.Text = RuleTemplateDetails.RLT_UIExpression;

                LoadExpressionObjects();
                txtValidationResult.Text = String.Empty;
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

        protected void ddlObjectType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (ddlObjectType.SelectedValue == ObjectType.Series_Item.GetStringValue())
                {
                    dvSeriesItem.Visible = true;
                    cmbSeriesItems.Visible = true;
                    cmbSeriesItems.DataSource = listItemSeriesItem;
                    cmbSeriesItems.DataValueField = "ISI_ID";
                    cmbSeriesItems.DataTextField = "ISI_ItemName";
                    cmbSeriesItems.DataBind();
                }
                else
                {
                    cmbSeriesItems.Items.Clear();
                    dvSeriesItem.Visible = false;
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

        #region Methods
        #region Private Methods

        private void BindRuleInfo()
        {
            Presenter.getRuleInfo();
            if (CurrentViewContext.RuleMapping != null)
            {
                Presenter.GetRuleTemplates();
                BindTemplates();
                BindSeriesObjectAndItems();

                cmbMasterTemplates.SelectedValue = Convert.ToString(CurrentViewContext.RuleMapping.RLM_RuleTemplateID);
                txtRuleName.Text = CurrentViewContext.RuleMapping.RLM_Name;
                txtActionMapping.Text = CurrentViewContext.RuleMapping.RLM_ActionBlock;
                txtSucessMessage.Text = CurrentViewContext.RuleMapping.RLM_SuccessMessage;
                chkActive.Checked = CurrentViewContext.RuleMapping.RLM_IsActive;
                txtErrorMessage.Text = CurrentViewContext.RuleMapping.RLM_ErrorMessage;
                txtValidationResult.Text = CurrentViewContext.RuleMapping.RLM_UIExpression;
                CurrentViewContext.SelectedRuleTemplateId = CurrentViewContext.RuleMapping.RLM_RuleTemplateID;

                Presenter.GetRuleTemplateDetails();
                CurrentViewContext.ObjectCount = CurrentViewContext.RuleTemplateDetails.RLT_ObjectCount;
                lblAction.Text = CurrentViewContext.RuleTemplateDetails.lkpRuleActionType.ACT_Description;
                litExpression.Text = RuleTemplateDetails.RLT_UIExpression;
                CurrentViewContext.FirstVersionRuleId = CurrentViewContext.RuleMapping.RLM_FirstVersionID.HasValue ? CurrentViewContext.RuleMapping.RLM_FirstVersionID : null;
                RuleSetId = CurrentViewContext.RuleMapping.RLM_RuleSetID;
                //LoadExpressionObjects();
            }
        }

        private void BindSeriesObjectAndItems()
        {
            BindObjectType();
            List<RuleSetObject> lstRuleSetObject = CurrentViewContext.RuleMapping.RuleSet.RuleSetObjects.Where(cond => !cond.RLSO_IsDeleted).ToList();
            SelectedObjectTypeCode = lstRuleSetObject.FirstOrDefault().lkpObjectType.OT_Code;
            if (SelectedObjectTypeCode == ObjectType.Series_Item.GetStringValue())
            {
                dvSeriesItem.Visible = true;
                cmbSeriesItems.Visible = true;
                cmbSeriesItems.DataSource = listItemSeriesItem;
                cmbSeriesItems.DataValueField = "ISI_ID";
                cmbSeriesItems.DataTextField = "ISI_ItemName";
                cmbSeriesItems.DataBind();

                List<Int32> selectedItemIds = lstRuleSetObject.Select(sel => sel.RLSO_ObjectID).ToList();
                foreach (RadComboBoxItem item in cmbSeriesItems.Items)
                {
                    item.Checked = selectedItemIds.Contains(Convert.ToInt32(item.Value));
                }
            }
            else
            {
                cmbSeriesItems.Items.Clear();
                dvSeriesItem.Visible = false;
            }
        }

        private void BindTemplates()
        {
            cmbMasterTemplates.DataSource = lstRuleTemplates;
            cmbMasterTemplates.DataTextField = "RLT_Name";
            cmbMasterTemplates.DataValueField = "RLT_ID";
            cmbMasterTemplates.DataBind();
            cmbMasterTemplates.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem { Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.ZERO });
        }

        private void LoadExpressionObjects()
        {
            List<lkpRuleObjectMappingType> listRuleObjectMappingType = Presenter.GetRuleObjectMappingType();
            if (CurrentViewContext.ObjectCount > 0)
            {
                if (CurrentViewContext.RuleMapping == null)
                {
                    for (int rowId = 1; rowId <= ObjectCount; rowId++)
                    {
                        ShotSeriesExpressionObject ctrl = (ShotSeriesExpressionObject)LoadControl("~/ComplianceAdministration/UserControl/ShotSeriesExpressionObject.ascx");
                        ctrl.RowId = rowId;
                        ctrl.ID = String.Format("EO_{0}", rowId);
                        ctrl.SelectedTenantId = SelectedTenantId;
                        ctrl.complinceDataStore = complinceDataStore;
                        ctrl.lstRuleObjectMappingType = listRuleObjectMappingType;
                        ctrl.categoryId = CategoryId;
                        pnlExpressionObjects.Controls.Add(ctrl);
                    }
                    return;
                }
                Int32 count = 1;
                foreach (RuleMappingDetail detail in CurrentViewContext.RuleMapping.RuleMappingDetails.Where(x => !x.RLMD_IsDeleted).OrderBy(x => { return Convert.ToInt32(x.RLMD_PlaceHolderName.Replace("[Object", "").Replace("]", "")); }))
                {
                    if (detail != null)
                    {

                        String ruleMappingObjectTypeCode = String.Empty;
                        String selectedObjectTypeCode = String.Empty;
                        Int32? selectedCategoryId = new Int32?();
                        Int32? selectedItemId = new Int32?();
                        Int32? selectedAttributeId = new Int32?();
                        String constantValue = String.Empty;
                        Int32? constantTypeId = new Int32?();
                        Int32? itemSeriesAttributeId = new Int32?();
                        if (detail.RLMD_RuleObjectMappingTypeID != null)
                        {
                            ruleMappingObjectTypeCode = listRuleObjectMappingType.FirstOrDefault(x => x.RMT_ID == detail.lkpRuleObjectMappingType.RMT_ID).RMT_Code;
                            if (ObjectMappingType.Compliance_Value.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                selectedObjectTypeCode = detail.lkpObjectType.OT_Code;
                                RuleMappingObjectTree ruleMappingObjectTreeForCategory = detail.RuleMappingObjectTrees.FirstOrDefault(x => x.RuleSetTree.RST_ObjectName == RuleSetTreeType.Categories.ToString());
                                if (ruleMappingObjectTreeForCategory != null)
                                {
                                    selectedCategoryId = ruleMappingObjectTreeForCategory.RMOT_ObjectID;
                                }
                                selectedItemId = detail.RLMD_ObjectID.Value;
                            }

                            else if (ObjectMappingType.Data_Value.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                selectedObjectTypeCode = detail.lkpObjectType.OT_Code;
                                RuleMappingObjectTree ruleMappingObjectTreeForCategory = detail.RuleMappingObjectTrees.FirstOrDefault(x => x.RuleSetTree.RST_ObjectName == RuleSetTreeType.Categories.ToString());
                                RuleMappingObjectTree ruleMappingObjectTreeForItem = detail.RuleMappingObjectTrees.FirstOrDefault(x => x.RuleSetTree.RST_ObjectName == RuleSetTreeType.Items.ToString());
                                if (ruleMappingObjectTreeForCategory != null)
                                {
                                    selectedCategoryId = ruleMappingObjectTreeForCategory.RMOT_ObjectID;
                                }
                                if (ruleMappingObjectTreeForItem != null)
                                {
                                    selectedItemId = ruleMappingObjectTreeForItem.RMOT_ObjectID;
                                }
                                selectedAttributeId = detail.RLMD_ObjectID.Value;
                            }

                            else if (ObjectMappingType.Defined_Value.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                constantValue = detail.RLMD_ConstantValue;
                                if (detail.RLMD_ConstantType != null)
                                {
                                    constantTypeId = detail.RLMD_ConstantType;
                                }
                            }

                            else if (ObjectMappingType.Series_Item_Status.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                selectedObjectTypeCode = detail.lkpObjectType.OT_Code;
                            }

                            else if (ObjectMappingType.Series_Item_Attribute.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                selectedObjectTypeCode = detail.lkpObjectType.OT_Code;
                                itemSeriesAttributeId = detail.RLMD_ObjectID;
                            }
                        }
                        object[] parameters = new object[] { ruleMappingObjectTypeCode, selectedObjectTypeCode, selectedCategoryId, selectedItemId, selectedAttributeId };
                        ShotSeriesExpressionObject expressionObject = (ShotSeriesExpressionObject)LoadControl("~/ComplianceAdministration/UserControl/ShotSeriesExpressionObject.ascx");

                        expressionObject.RowId = count;
                        expressionObject.ID = String.Format("EO_{0}", count);
                        expressionObject.SelectedTenantId = SelectedTenantId;
                        expressionObject.CategoryId = CategoryId;
                        expressionObject.ObjectName = detail.RLMD_PlaceHolderName;
                        expressionObject.SelectedMappingTypeCode = ruleMappingObjectTypeCode;
                        expressionObject.objectTypeCode = selectedObjectTypeCode;
                        expressionObject.categoryId = selectedCategoryId;
                        expressionObject.itemId = selectedItemId;
                        expressionObject.attributeId = selectedAttributeId;
                        expressionObject.itemSeriesAttributeId = itemSeriesAttributeId;
                        expressionObject.SeriesId = CurrentViewContext.SeriesId;
                        pnlExpressionObjects.Controls.Add(expressionObject);
                        expressionObject.ConstantValue = constantValue;
                        expressionObject.constantTypeId = constantTypeId;
                        expressionObject.complinceDataStore = complinceDataStore;
                        expressionObject.lstRuleObjectMappingType = listRuleObjectMappingType;
                        count++;
                    }
                }
            }
        }

        private RuleMapping GetRuleMapping()
        {
            RuleMapping mapping = new RuleMapping();
            mapping.RLM_ID = CurrentViewContext.RuleMappingId;
            mapping.RLM_RuleTemplateID = Convert.ToInt32(cmbMasterTemplates.SelectedValue);
            mapping.RLM_SuccessMessage = txtSucessMessage.Text;
            mapping.RLM_ErrorMessage = txtErrorMessage.Text;
            mapping.RLM_RuleSetID = RuleSetId;
            mapping.RLM_Name = txtRuleName.Text;
            mapping.RLM_ActionBlock = txtActionMapping.Text;
            //mapping.RLM_UIExpression = uiExpression;
            mapping.RLM_IsActive = true;
            mapping.RLM_IsDeleted = false;
            //mapping.RLM_ModifiedByID = CurrentLoggedInUserId;
            //mapping.RLM_ModifiedOn = DateTime.Now;
            mapping.RLM_IsActive = chkActive.Checked;
            foreach (RuleMappingDetail mappingDetail in GetRuleMappingDetail())
                mapping.RuleMappingDetails.Add(mappingDetail);

            return mapping;
        }

        private List<RuleMappingDetail> GetRuleMappingDetail()
        {
            List<RuleMappingDetail> mappingDetails = new List<RuleMappingDetail>();
            List<lkpObjectType> objectTypeList = Presenter.GetObjectTypeList();
            List<RuleSetTree> ruleSetTree = Presenter.GetRuleSetTreeData();
            // RuleMappingObjectTree ruleMappingObjectTreeForCurrentPackage = GetRuleMappingObjectTree(PackageId, RuleSetTreeType.Packages, ruleSetTree);
            for (Int32 counter = 1; counter <= ObjectCount; counter++)
            {
                ShotSeriesExpressionObject expressionObject = (ShotSeriesExpressionObject)pnlExpressionObjects.FindControl(String.Format("EO_{0}", counter));
                if (expressionObject != null)
                {
                    RuleMappingDetail detail = new RuleMappingDetail();
                    detail.RLMD_PlaceHolderName = expressionObject.ObjectName;
                    detail.RLMD_CreatedByID = CurrentLoggedInUserId;
                    detail.RLMD_CreatedOn = DateTime.Now;
                    detail.RLMD_IsDeleted = false;
                    detail.IfExpressionIsvalid = expressionObject.IsExpressionObjectIsValid();
                    if (detail.IfExpressionIsvalid)
                    {
                        if (ObjectMappingType.Compliance_Value.GetStringValue().Equals(expressionObject.SelectedMappingTypeCode))
                        {
                            detail.RLMD_ObjectID = expressionObject.SelectedItemId;

                            lkpObjectType objectType = objectTypeList.FirstOrDefault(x => x.OT_Code == expressionObject.SelectedObjectTypeCode);
                            detail.RLMD_ObjectTypeID = objectType.OT_ID;

                            lkpRuleObjectMappingType ruleObjectMappingType = listRuleObjectMappingType.FirstOrDefault(x => x.RMT_Code == expressionObject.SelectedMappingTypeCode);
                            detail.RLMD_RuleObjectMappingTypeID = ruleObjectMappingType.RMT_ID;

                            RuleMappingObjectTree ruleMappingObjectTreeForCategory = GetRuleMappingObjectTree(expressionObject.SelectedCategoryId, RuleSetTreeType.Categories, ruleSetTree);
                            if (ruleMappingObjectTreeForCategory != null)
                                detail.RuleMappingObjectTrees.Add(ruleMappingObjectTreeForCategory);
                        }
                        else if (ObjectMappingType.Data_Value.GetStringValue().Equals(expressionObject.SelectedMappingTypeCode))
                        {
                            detail.RLMD_ObjectID = expressionObject.SelectedAttributeId;

                            lkpObjectType objectType = objectTypeList.FirstOrDefault(x => x.OT_Code == expressionObject.SelectedObjectTypeCode); ;
                            detail.RLMD_ObjectTypeID = objectType.OT_ID;

                            lkpRuleObjectMappingType ruleObjectMappingType = listRuleObjectMappingType.FirstOrDefault(x => x.RMT_Code == expressionObject.SelectedMappingTypeCode);
                            detail.RLMD_RuleObjectMappingTypeID = ruleObjectMappingType.RMT_ID;

                            RuleMappingObjectTree ruleMappingObjectTreeForCategory = GetRuleMappingObjectTree(expressionObject.SelectedCategoryId, RuleSetTreeType.Categories, ruleSetTree);
                            if (ruleMappingObjectTreeForCategory != null)
                                detail.RuleMappingObjectTrees.Add(ruleMappingObjectTreeForCategory);

                            RuleMappingObjectTree ruleMappingObjectTreeForItem = GetRuleMappingObjectTree(expressionObject.SelectedItemId, RuleSetTreeType.Items, ruleSetTree);
                            if (ruleMappingObjectTreeForItem != null)
                                detail.RuleMappingObjectTrees.Add(ruleMappingObjectTreeForItem);

                        }
                        else if (ObjectMappingType.Defined_Value.GetStringValue().Equals(expressionObject.SelectedMappingTypeCode))
                        {
                            detail.RLMD_ObjectID = null;
                            detail.RLMD_ObjectTypeID = null;

                            lkpRuleObjectMappingType ruleObjectMappingType = listRuleObjectMappingType.FirstOrDefault(x => x.RMT_Code == expressionObject.SelectedMappingTypeCode);
                            detail.RLMD_RuleObjectMappingTypeID = ruleObjectMappingType.RMT_ID;
                            lkpConstantType constantType = ConstantTypeList.FirstOrDefault(x => x.Code == expressionObject.selectedConstantTypeCode);
                            detail.RLMD_ConstantType = constantType.ID;
                            detail.RLMD_ConstantValue = expressionObject.ConstantValue;
                        }
                        else
                        {
                            if (ObjectMappingType.Series_Item_Status.GetStringValue().Equals(expressionObject.SelectedMappingTypeCode))
                                detail.RLMD_ObjectID = null;

                            else
                                detail.RLMD_ObjectID = Convert.ToInt32(expressionObject.SelectedItemSeriesAttributeId);

                            lkpObjectType objectType = objectTypeList.FirstOrDefault(x => x.OT_Code == expressionObject.SelectedObjectTypeCode);
                            detail.RLMD_ObjectTypeID = objectType.OT_ID;

                            lkpRuleObjectMappingType ruleObjectMappingType = listRuleObjectMappingType.FirstOrDefault(x => x.RMT_Code == expressionObject.SelectedMappingTypeCode);
                            detail.RLMD_RuleObjectMappingTypeID = ruleObjectMappingType.RMT_ID;
                        }
                    }
                    mappingDetails.Add(detail);
                }
            }
            return mappingDetails;
        }

        private RuleMappingObjectTree GetRuleMappingObjectTree(Int32 objectID, RuleSetTreeType ruleSetTreeType, List<RuleSetTree> ruleSetTreeData)
        {
            RuleSetTree ruleSetTree = ruleSetTreeData.FirstOrDefault(x => x.RST_UICode == ruleSetTreeType.GetStringValue());
            if (ruleSetTree == null)
                return null;
            RuleMappingObjectTree ruleMappingObjectTree = new RuleMappingObjectTree();
            ruleMappingObjectTree.RMOT_RuleSetTreeID = ruleSetTree.RST_ID;
            ruleMappingObjectTree.RMOT_ObjectID = objectID;
            return ruleMappingObjectTree;
        }


        #region Validate Rule
        /// <summary>
        /// Method used for validating rule
        /// </summary>
        /// <returns>Rule Processing Result</returns>
        private RuleProcessingResult validateRule(List<RuleMappingDetail> ruleMappingList)
        {
            String ruleTemplateXml = generateTemplateExpressionXml();
            String ruleExpressionXml = generateMappingXml(ruleMappingList);

            var xmlOutputString = Presenter.ValidateRule(ruleTemplateXml, ruleExpressionXml);
            return xmlOutputString;
        }

        /// <summary>
        /// Method for generating object Mapping Xml
        /// </summary>
        /// <returns></returns>
        private String generateMappingXml(List<RuleMappingDetail> mappingDetails)
        {
            List<Int32> objectIds = new List<Int32>();
            List<ComplianceAttribute> attributeDetail = new List<ComplianceAttribute>();
            foreach (RuleMappingDetail detail in mappingDetails)
            {
                if (detail.RLMD_RuleObjectMappingTypeID.IsNull())
                {
                    continue;
                }
                if (listRuleObjectMappingType.FirstOrDefault(x => x.RMT_ID == detail.RLMD_RuleObjectMappingTypeID.Value).RMT_Code == "DVAL")
                {
                    objectIds.Add(detail.RLMD_ObjectID.Value);
                }
            }
            if (objectIds.Count > 0)
            {
                attributeDetail = Presenter.getAttributeDetail(objectIds);
            }

            List<ItemSeriesAttribute> lstItemSeriesAttribute = Presenter.GetItemSeriesAttributeBySeriesId();

            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("ObjectMapping"));
            XmlNode expGroup = el.AppendChild(doc.CreateElement("Mappings"));

            //for intializing object counter
            Int32 counter = 1;
            foreach (RuleMappingDetail detail in mappingDetails)
            {
                ShotSeriesExpressionObject expressionObject = (ShotSeriesExpressionObject)pnlExpressionObjects.FindControl(String.Format("EO_{0}", counter));

                if (detail.RLMD_RuleObjectMappingTypeID.IsNull())
                {
                    continue;
                }
                XmlNode exp = expGroup.AppendChild(doc.CreateElement("Mapping"));
                exp.AppendChild(doc.CreateElement("Key")).InnerText = detail.RLMD_PlaceHolderName;
                if (expressionObject.SelectedMappingTypeCode == "COMPL")
                {
                    exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.Boolean.GetStringValue();
                    exp.AppendChild(doc.CreateElement("MappedName")).InnerText = expressionObject.SelectedItemName;
                }
                else if (expressionObject.SelectedMappingTypeCode == "CONST")
                {
                    // exp.AppendChild(doc.CreateElement("DataType")).InnerText = GetOperandType(detail.RLMD_ConstantValue).GetStringValue();
                    //when current Day is selected as a constant
                    if (expressionObject.selectedConstantTypeCode == ConstantType.Day.GetStringValue() && expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.Date.GetStringValue();
                    }

                    //when current Month or Year is selected as a constant
                    else if ((expressionObject.selectedConstantTypeCode == ConstantType.Month.GetStringValue()
                         || expressionObject.selectedConstantTypeCode == ConstantType.Year.GetStringValue())
                        && expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.Numeric.GetStringValue();
                    }

                    //when Empty is selected as a constant
                    else if (expressionObject.ConstantValue == AppConsts.EMPTY)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.Text.GetStringValue();
                    }

                    //when manual Day is selected as a constant
                    else if (expressionObject.selectedConstantTypeCode == ConstantType.Day.GetStringValue() && !expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.TimeSpanDay.GetStringValue();
                    }

                    //when manual mnth is selected as a constant
                    else if (expressionObject.selectedConstantTypeCode == ConstantType.Month.GetStringValue() && !expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.TimeSpanMnth.GetStringValue();
                    }

                     //when manual Year is selected as a constant 
                    else if (expressionObject.selectedConstantTypeCode == ConstantType.Year.GetStringValue() && !expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.TimeSpanYear.GetStringValue();
                    }

                    //when date of birth selected as a constant 
                    else if (expressionObject.selectedConstantTypeCode == ConstantType.DOB.GetStringValue())
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.Date.GetStringValue();
                    }
                    else
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = GetOperandType(detail.RLMD_ConstantValue).GetStringValue();
                    }
                    exp.AppendChild(doc.CreateElement("MappedName")).InnerText = expressionObject.ConstantValue;
                }
                else if (expressionObject.SelectedMappingTypeCode == "DVAL")
                {
                    String datatype = attributeDetail.Where(obj => obj.ComplianceAttributeID == detail.RLMD_ObjectID).FirstOrDefault().lkpComplianceAttributeDatatype.Name;
                    if (datatype == "Options")
                    {
                        datatype = OperandType.Text.GetStringValue();
                    }
                    if (datatype == "File Upload" || datatype == "View Document")
                    {
                        datatype = OperandType.Numeric.GetStringValue();
                    }
                    exp.AppendChild(doc.CreateElement("DataType")).InnerText = datatype;
                    exp.AppendChild(doc.CreateElement("MappedName")).InnerText = expressionObject.SelectedAttributeName;
                }

                else if (expressionObject.SelectedMappingTypeCode == ObjectMappingType.Series_Item_Status.GetStringValue())
                {
                    exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.Numeric.GetStringValue();
                    exp.AppendChild(doc.CreateElement("MappedName")).InnerText = expressionObject.SelectedObjectTypeName;
                }

                else if (expressionObject.SelectedMappingTypeCode == ObjectMappingType.Series_Item_Attribute.GetStringValue())
                {
                    var ItemSeriesAttribute = lstItemSeriesAttribute.FirstOrDefault(cond => cond.ISA_ID == expressionObject.SelectedItemSeriesAttributeId);
                    String datatype = ItemSeriesAttribute.ComplianceAttribute.lkpComplianceAttributeDatatype.Name;
                    if (datatype == "Options")
                    {
                        datatype = OperandType.Text.GetStringValue();
                    }
                    if (datatype == "File Upload" || datatype == "View Document")
                    {
                        datatype = OperandType.Numeric.GetStringValue();
                    }
                    exp.AppendChild(doc.CreateElement("DataType")).InnerText = datatype;
                    exp.AppendChild(doc.CreateElement("MappedName")).InnerText = expressionObject.SelectedSeriesAttributeName;
                }
                counter++;
            }
            return doc.OuterXml.ToString();
        }

        /// <summary>
        /// Method for generating template Xml
        /// </summary>
        /// <returns></returns>
        private String generateTemplateExpressionXml()
        {
            CurrentViewContext.SelectedRuleTemplateId = Convert.ToInt32(cmbMasterTemplates.SelectedValue);
            Presenter.GetRuleTemplateDetails();
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("Rule"));
            el.AppendChild(doc.CreateElement("ResultType")).InnerText = RuleTemplateDetails.lkpRuleResultType.RSL_Code;
            XmlNode expGroup = el.AppendChild(doc.CreateElement("Expressions"));

            //UAT-2684:Fix "Mapping not provided for token E#" issue
            List<RuleTemplateExpression> lstRuleTemplateExpression = RuleTemplateDetails.RuleTemplateExpressions.Where(x => !x.RLE_IsDeleted).OrderBy(ord => ord.RLE_ExpressionOrder).ToList();
            foreach (var expression in lstRuleTemplateExpression)
            {
                XmlNode exp = expGroup.AppendChild(doc.CreateElement("Expression"));

                string expressionName = expression.Expression.EX_Name == "(Group)" ? "GE" : expression.Expression.EX_Name;
                exp.AppendChild(doc.CreateElement("Name")).InnerText = expressionName;

                string definition = expression.Expression.EX_Expression;
                exp.AppendChild(doc.CreateElement("Definition")).InnerText = "<![CDATA[" + definition.Trim() + "]]>";
            }

            return (doc.OuterXml.ToString().Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&"));
        }

        /// <summary>
        /// To get Operant Data Type
        /// </summary>
        /// <param name="expressionOperatorName"></param>
        /// <returns></returns>
        public static OperandType GetOperandType(string operand)
        {
            try
            {
                Decimal tempD = Decimal.Parse(operand);
                return OperandType.Numeric;
            }
            catch
            {
                try
                {
                    Boolean tempB = Boolean.Parse(operand);
                    return OperandType.Boolean;
                }
                catch
                {
                    try
                    {
                        DateTime tempD = DateTime.Parse(operand);
                        return OperandType.Date;
                    }
                    catch { }
                }
            }
            return OperandType.Text;
        }
        #endregion

        private void SetFormMode(Boolean isEnabled)
        {
            ddlObjectType.Enabled = false;
            cmbSeriesItems.Enabled = isEnabled;
            cmbMasterTemplates.Enabled = isEnabled;
            txtRuleName.Enabled = isEnabled;
            lblAction.Enabled = isEnabled;
            txtActionMapping.Enabled = isEnabled;
            chkActive.IsActiveEnable = isEnabled;
            txtErrorMessage.Enabled = isEnabled;
            txtSucessMessage.Enabled = isEnabled;
            pnlExpressionObjects.Enabled = isEnabled;
            btnValidate.Enabled = isEnabled;
            txtValidationResult.Enabled = isEnabled;
            ddlTestUsersList.Enabled = isEnabled;
            txtTestResult.Enabled = isEnabled;
        }

        private void ResetButtons(Boolean isReset)
        {
            fsucCmdBarRule.SaveButton.Visible = !isReset;
            fsucCmdBarRule.CancelButton.Visible = !isReset;
            fsucCmdBarRule.SubmitButton.Visible = isReset;
            fsucCmdBarRule.SubmitButton.Text = "Edit";
            fsucCmdBarRule.ClearButton.Visible = false;
            if (IsVersionUpdate)
            {
                fsucCmdBarRule.SaveButton.Text = "Save";
            }
        }

        private String constantInput(RuleMappingDetail ruleMapping)
        {
            String errorMsg = String.Empty;

            lkpConstantType constantType = ConstantTypeList.FirstOrDefault(x => x.ID == ruleMapping.RLMD_ConstantType);
            if (constantType.Code != String.Empty)
            {
                if (ruleMapping.RLMD_ConstantValue == AppConsts.CURRENT_DAY || ruleMapping.RLMD_ConstantValue == AppConsts.CURRENT_MONTH
                    || ruleMapping.RLMD_ConstantValue == AppConsts.CURRENT_YEAR || ruleMapping.RLMD_ConstantValue == AppConsts.EMPTY
                    || constantType.Code == ConstantType.DOB.GetStringValue())
                {
                    return errorMsg;
                }
                else
                {
                    if (constantType.Code == ConstantType.Bool.GetStringValue())
                    {
                        try
                        {
                            Boolean tempD = Boolean.Parse(ruleMapping.RLMD_ConstantValue);
                        }
                        catch
                        {
                            errorMsg += String.Format("\"" + ruleMapping.RLMD_ConstantValue + "\" is not an appropriate value for constant type \"" + constantType.Name + "\".\n");
                        }
                    }
                    else if (constantType.Code == ConstantType.Numeic.GetStringValue())
                    {
                        try
                        {
                            Decimal tempD = Decimal.Parse(ruleMapping.RLMD_ConstantValue);
                        }
                        catch
                        {
                            errorMsg += String.Format("\"" + ruleMapping.RLMD_ConstantValue + "\" is not an appropriate value for constant type \"" + constantType.Name + "\".\n");
                        }

                    }
                    else if (constantType.Code == ConstantType.Date.GetStringValue())
                    {
                        try
                        {
                            DateTime tempD = DateTime.Parse(ruleMapping.RLMD_ConstantValue);
                        }
                        catch
                        {
                            errorMsg += String.Format("\"" + ruleMapping.RLMD_ConstantValue + "\" is not an appropriate value for constant type \"" + constantType.Name + "\".\n");
                        }
                    }
                    else if (constantType.Code == ConstantType.Day.GetStringValue()
                        || constantType.Code == ConstantType.Month.GetStringValue()
                        || constantType.Code == ConstantType.Year.GetStringValue())
                    {
                        try
                        {
                            Int32 tempD = Int32.Parse(ruleMapping.RLMD_ConstantValue);
                        }
                        catch
                        {
                            errorMsg += String.Format("\"" + ruleMapping.RLMD_ConstantValue + "\" is not an appropriate value for constant type \"" + constantType.Name + "\".\n");
                        }
                    }
                }
                return errorMsg;
            }
            return errorMsg;
        }

        private void BindObjectType()
        {
            List<lkpObjectType> lstObjectType = Presenter.GetObjectTypeList().Where(cond => cond.OT_Code == "SRS" || cond.OT_Code == "SRITM").ToList();
            ddlObjectType.DataSource = lstObjectType;
            ddlObjectType.DataTextField = "OT_Name";
            ddlObjectType.DataValueField = "OT_Code";
            ddlObjectType.DataBind();
        }
        #endregion


        #endregion
    }
}
