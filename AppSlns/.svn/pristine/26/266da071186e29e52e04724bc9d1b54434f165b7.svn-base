using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
using System.Web.UI;
using CoreWeb.Shell;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTSOF.Utils;
using System.Text;
using Telerik.Web.UI;
using System.Xml;
using System.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.SharedObjects;
using CoreWeb.IntsofSecurityModel;
using System.Web.UI.WebControls;



namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ShotSeriesRuleList : BaseWebPage, IShotSeriesRuleListView
    {
        #region Variable

        #region Private Variables
        private ShotSeriesRuleListPresenter _presenter = new ShotSeriesRuleListPresenter();
        Int32 tenantid = 0;
        private ComplianceDataStore _complinceDataStore;
        private List<lkpRuleObjectMappingType> _listRuleObjectMappingType;
        private List<lkpConstantType> _constantTypeList;
        private Boolean _isScheduleActionRecordInserted = false;
        private List<ItemSeriesItem> _listItemSeriesItem;
        #endregion

        #region Public variables

        #endregion

        #endregion

        #region Properties

        public ShotSeriesRuleListPresenter Presenter
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

        public List<RuleTemplate> lstRuleTemplates
        {
            get;
            set;
        }

        public IShotSeriesRuleListView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 CategoryId
        {
            get
            {
                return Convert.ToInt32(ViewState["vwCategoryId"]);
            }
            set
            {
                ViewState["vwCategoryId"] = value;
            }
        }

        public Int32 RuleSetId
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

        public RuleTemplate RuleTemplateDetails
        {
            get;
            set;
        }

        public Int32 SelectedRuleTemplateId
        {
            get;
            set;
        }

        public List<RuleMapping> lstRuleMapping
        {
            get;
            set;

        }

        public Int32 RuleMappingId
        {
            get;
            set;
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

        public Int32 TenantId
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

        Int32 IShotSeriesRuleListView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String Expression { get; set; }

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

        ComplianceDataStore ComplinceDataStore
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


        public String ErrMsg
        {
            get;
            set;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();

                    if (Request.QueryString["Id"] != null)
                        SeriesId = Convert.ToInt32(Request.QueryString["Id"]);

                    if (Request.QueryString["SelectedTenantId"] != null)
                    {
                        SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    }

                    if (Request.QueryString["CategoryId"] != null)
                    {
                        CategoryId = Convert.ToInt32(Request.QueryString["CategoryId"]);
                    }
                    Presenter.GetRuleTemplates();
                    BindTemplates();

                    chkActive.Checked = true;
                    if (SelectedTenantId == DefaultTenantId)
                        ((GridButtonColumn)grdRules.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Are you sure you want to delete this record?";
                    else
                        ((GridButtonColumn)grdRules.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Deleting this record will impact existing orders. Are you sure you want to delete?";
                    BindObjectType();
                }
                LoadExpressionObjects();

                lblSuccess.Visible = false;
                lblSuccess.Text = String.Empty;

                Presenter.OnViewLoaded();
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        protected void grdRules_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                CurrentViewContext.RuleSetId = RuleSetId;
                Presenter.GetRuleMappings();
                grdRules.DataSource = lstRuleMapping;
                grdRules.Columns.FindByUniqueName("RLM_IsCurrent").Visible = !DefaultTenantId.Equals(SelectedTenantId);
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        protected void grdRules_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.RuleMappingId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RLM_ID"]);
                    if (Presenter.DeleteRuleMapping())
                    {
                        grdRules.Rebind();
                        lblSuccess.Visible = true;
                        lblSuccess.ShowMessage("Rule deleted successfully.", MessageType.SuccessMessage);
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    else
                    {
                        lblSuccess.Visible = true;
                        lblSuccess.ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                    }
                }
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
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

        protected void cmdBarSaveRule_SaveClick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cmbMasterTemplates.SelectedValue))
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
                        errorMsg += validateConstantInput(ruleMapping);
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

                        Presenter.SaveRuleMapping(mappingToBeSaved);
                        String settingsXml = String.Empty;

                        grdRules.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                        ResetControls();
                        divAddForm.Visible = false;
                        lblSuccess.Visible = true;
                        if (errorMsg == String.Empty)
                            lblSuccess.ShowMessage("Rule saved successfully.", MessageType.SuccessMessage);
                        else
                            lblSuccess.ShowMessage(ErrMsg, MessageType.Information);
                    }
                }
            }
        }

        protected void cmdBarSaveRule_CancelClick(object sender, EventArgs e)
        {
            divAddForm.Visible = false;

        }

        protected void btnValidate_Click(object sender, EventArgs e)
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
                    errorMsg += validateConstantInput(ruleMapping);
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = true;
            ResetControls();
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

        #region Methods
        #region Private Methods
        private RuleMapping GetRuleMapping()
        {
            RuleMapping mapping = new RuleMapping();
            mapping.RLM_RuleTemplateID = Convert.ToInt32(cmbMasterTemplates.SelectedValue);
            mapping.RLM_SuccessMessage = txtSucessMessage.Text;
            mapping.RLM_ErrorMessage = txtErrorMessage.Text;
            //mapping.RLM_RuleSetID = RuleSetId;
            mapping.RLM_Name = txtRuleName.Text;
            mapping.RLM_ActionBlock = txtActionMapping.Text;
            mapping.RLM_IsActive = true;
            mapping.RLM_IsDeleted = false;
            mapping.RLM_CreatedByID = TenantId;
            mapping.RLM_CreatedOn = DateTime.Now;
            mapping.RLM_IsActive = chkActive.Checked;
            mapping.RLM_Code = Guid.NewGuid();
            foreach (RuleMappingDetail mappingDetail in GetRuleMappingDetail())
                mapping.RuleMappingDetails.Add(mappingDetail);

            return mapping;
        }

        private List<RuleMappingDetail> GetRuleMappingDetail()
        {
            List<RuleMappingDetail> mappingDetails = new List<RuleMappingDetail>();
            List<lkpObjectType> objectTypeList = Presenter.GetObjectTypeList();
            List<RuleSetTree> ruleSetTree = Presenter.GetRuleSetTreeData();
            //RuleMappingObjectTree ruleMappingObjectTreeForCurrentPackage = GetRuleMappingObjectTree(PackageId, RuleSetTreeType.Packages, ruleSetTree);
            for (Int32 counter = 1; counter <= ObjectCount; counter++)
            {
                ShotSeriesExpressionObject expressionObject = (ShotSeriesExpressionObject)pnlExpressionObjects.FindControl(String.Format("EO_{0}", counter));
                if (expressionObject != null)
                {
                    RuleMappingDetail detail = new RuleMappingDetail();
                    detail.RLMD_PlaceHolderName = expressionObject.ObjectName;
                    detail.RLMD_CreatedByID = tenantid;
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

                            lkpRuleObjectMappingType ruleObjectMappingType = listRuleObjectMappingType.FirstOrDefault(x => x.RMT_Code == expressionObject.SelectedMappingTypeCode); ;
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
                            //detail.RLMD_ConstantValue = expressionObject.ConstantValue;
                            lkpConstantType constantType = ConstantTypeList.FirstOrDefault(x => x.Code == expressionObject.selectedConstantTypeCode);
                            //detail.lkpConstantType = constantType;
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

        private void GetTemplates()
        {
            cmbMasterTemplates.DataSource = lstRuleTemplates;
            cmbMasterTemplates.DataTextField = "RLT_Name";
            cmbMasterTemplates.DataValueField = "RLT_ID";
            cmbMasterTemplates.DataBind();

            cmbMasterTemplates.Items.Add(new Telerik.Web.UI.RadComboBoxItem { Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.ZERO });
        }

        private RuleProcessingResult validateRule(List<RuleMappingDetail> ruleMappingList)
        {
            String ruleTemplateXml = generateTemplateExpressionXml();
            String ruleExpressionXml = generateMappingXml(ruleMappingList);

            var xmlOutputString = Presenter.ValidateRule(ruleTemplateXml, ruleExpressionXml);
            return xmlOutputString;
        }

        private void LoadExpressionObjects()
        {
            if (ObjectCount > 0)
            {
                //List<lkpRuleObjectMappingType> listRuleObjectMappingType = Presenter.GetRuleObjectMappingType();
                for (int rowId = 1; rowId <= ObjectCount; rowId++)
                {
                    ShotSeriesExpressionObject ctrl = (ShotSeriesExpressionObject)LoadControl("~/ComplianceAdministration/UserControl/ShotSeriesExpressionObject.ascx");
                    ctrl.RowId = rowId;
                    ctrl.ID = String.Format("EO_{0}", rowId);
                    ctrl.CategoryId = CategoryId;
                    ctrl.SelectedTenantId = SelectedTenantId;
                    ctrl.complinceDataStore = ComplinceDataStore;
                    ctrl.lstRuleObjectMappingType = listRuleObjectMappingType;
                    ctrl.SeriesId = CurrentViewContext.SeriesId;
                    pnlExpressionObjects.Controls.Add(ctrl);
                }
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


        /// <summary>
        /// Method for generating object Mapping Xml
        /// </summary>
        /// <returns></returns>
        private String generateMappingXml(List<RuleMappingDetail> mappingDetails)
        {
            // List<RuleMappingDetail> mappingDetails = GetRuleMappingDetail();
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

        private String replaceExpressionTextWithSymbol(String expression)
        {
            if (expression != String.Empty)
            {
                string[] expElements = expression.Split(' ');

                string firstObject = expElements[0];
                string thirdObject = expElements[expElements.Length - 1];

                Int32 i = 1;
                string finalExpression = string.Empty;
                while (i < expElements.Length - 1)
                {
                    finalExpression += expElements[i] + " ";
                    i++;
                }
                if (finalExpression.Trim() != String.Empty)
                {
                    String replacedExpression = finalExpression.Replace(finalExpression.Trim(), GetExpressionOperator(finalExpression.Trim().ToUpper()));
                    return expression.Replace(finalExpression, replacedExpression);
                }
            }
            return expression;
        }


        /// <summary>
        /// To get Mathmatical Expression Operator
        /// </summary>
        /// <param name="expressionOperatorName"></param>
        /// <returns></returns>
        private String GetExpressionOperator(String expressionOperatorName)
        {
            String expressionOperator = String.Empty;
            switch (expressionOperatorName)
            {
                case "PLUS":
                    expressionOperator = "+";
                    break;
                case "MINUS":
                    expressionOperator = "-";
                    break;
                case "MULTIPLY":
                    expressionOperator = "*";
                    break;
                case "DIVIDE":
                    expressionOperator = "/";
                    break;
                case "EQUAL":
                    expressionOperator = "=";
                    break;
                case "NOT EQUAL":
                    expressionOperator = "!=";
                    break;
                case "GREATER THAN":
                    expressionOperator = ">";
                    break;
                case "LESS THAN":
                    expressionOperator = "<";
                    break;
                case "GREATER THAN EQUAL TO":
                    expressionOperator = ">=";
                    break;
                case "LESS THAN EQUAL TO":
                    expressionOperator = "<=";
                    break;
                case "AND":
                    expressionOperator = "&&";
                    break;
                case "OR":
                    expressionOperator = "||";
                    break;
                case "NOT":
                    expressionOperator = "!";
                    break;
                default:
                    return expressionOperatorName + " ";

            }
            return expressionOperator;
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

        private void ResetControls()
        {
            txtRuleName.Text = String.Empty;
            txtSucessMessage.Text = String.Empty;
            txtErrorMessage.Text = String.Empty;
            txtActionMapping.Text = String.Empty;
            cmbMasterTemplates.SelectedValue = AppConsts.ZERO;
            litExpression.Text = String.Empty;
            txtValidationResult.Text = String.Empty;
            pnlExpressionObjects.Controls.Clear();
        }

        private String validateConstantInput(RuleMappingDetail ruleMapping)
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

