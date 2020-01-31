
#region NameSpace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.SharedObjects;
using System.Xml;
using INTSOF.UI.Contract.ComplianceManagement;
#endregion

namespace CoreWeb.BkgSetup.Views
{
    public partial class RuleInfoBkg : BaseUserControl, IRuleInfoBkgView
    {
        public delegate void NotifyStatusDelegate(String message, Boolean isSuccess);
        public event NotifyStatusDelegate NotifyStatusChange;
        public delegate void NotifyCancelClickDelegate();
        public event NotifyCancelClickDelegate NotifyCancelClick;
        #region Variable

        #region Private Variables
        private RuleInfoBkgPresenter _presenter = new RuleInfoBkgPresenter();
        Int32 tenantid = 0;
        private BkgDataStore _bkgDataStore;
        private List<lkpBkgRuleObjectMappingType> _listRuleObjectMappingType;
        private List<lkpBkgConstantType> _constantTypeList;
        #endregion
        #endregion

        #region Properties

        public RuleInfoBkgPresenter Presenter
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

        public IRuleInfoBkgView CurrentViewContext
        {
            get { return this; }
        }

        public int TenantId
        {
            get
            {
                if (tenantid == 0)
                {
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

        public List<BkgRuleTemplate> lstRuleTemplates
        {
            get;
            set;
        }

        public BkgRuleTemplate RuleTemplateDetails
        {
            get;
            set;

        }

        public BkgRuleMapping RuleMapping
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


        public List<lkpBkgRuleActionType> RuleActionTypes
        {
            set
            {
                ddlActionType.DataSource = value;
                ddlActionType.DataBind();
            }
        }

        public List<lkpBkgRuleType> RuleTypes
        {
            set
            {
                ddlRuleType.DataSource = value;
                ddlRuleType.DataBind();
            }
        }

        public Int32 PackageId
        {
            get
            {
                if (ViewState["PackageId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["PackageId"]);
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        BkgDataStore BackgroundDataStore
        {
            get
            {
                if (_bkgDataStore.IsNull())
                {
                    _bkgDataStore = new BkgDataStore();
                }
                return _bkgDataStore;
            }
            set
            {
                _bkgDataStore = value;
            }
        }

        List<lkpBkgRuleObjectMappingType> listRuleObjectMappingType
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

        List<lkpBkgConstantType> ConstantTypeList
        {
            get
            {
                if (_constantTypeList == null)
                {
                    _constantTypeList = Presenter.GetConstantType();
                }
                return _constantTypeList;
            }
            set
            {
                _constantTypeList = value;
            }
        }
        #endregion
        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    BindTemplates();
                    Presenter.GetRuleType();
                    Presenter.GetRuleActionType();
                    if (RuleMappingId != AppConsts.NONE)
                    {
                        BindRuleInfo();
                        EnableDisableControls(false);
                    }
                    else
                    {
                        chkActive.Checked = true;
                        EnableDisableControls(true);
                    }
                    ResetButtons(CurrentViewContext.RuleMappingId != AppConsts.NONE);
                }
                if (hdnIsCancelRequest.Value != "" && Convert.ToBoolean(hdnIsCancelRequest.Value))
                {
                    //do nothing because we need to load fresh data from db.
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

        #region DropDownList Events
        protected void cmbMasterTemplates_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                pnlExpressionObjects.Controls.Clear();
                txtValidationResult.Text = String.Empty;
                if (cmbMasterTemplates.SelectedValue == "0")
                {
                    litExpression.Text = string.Empty;
                    return;
                }
                CurrentViewContext.SelectedRuleTemplateId = Convert.ToInt32(cmbMasterTemplates.SelectedValue);
                Presenter.GetRuleTemplateDetails();
                ObjectCount = CurrentViewContext.RuleTemplateDetails.BRLT_ObjectCount;
                litExpression.Text = RuleTemplateDetails.BRLT_UIExpression;
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
        #endregion

        #region Button Events

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMasterTemplates.SelectedValue == AppConsts.ZERO)
                {
                    txtValidationResult.Text = String.Format(" Please specify Template");
                    return;
                }
                List<BkgRuleMappingDetail> ruleMappingList = GetRuleMappingDetail();
                String errorMsg = String.Empty;
                foreach (BkgRuleMappingDetail ruleMapping in ruleMappingList)
                {
                    if (!ruleMapping.IfExpressionIsvalid)
                    {
                        errorMsg += String.Format(" Please specify mapping for  \"" + ruleMapping.BRLMD_PlaceHolderName + "\".\n");
                    }
                    else if (ruleMapping.BRLMD_ConstantValue != String.Empty && ruleMapping.BRLMD_ConstantValue != null)
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

        protected void cmdBarRule_SaveClick(object sender, EventArgs e)
        {
            try
            {
                BkgRuleMapping mappingToBeSaved = GetRuleMapping();
                String errorMsg = String.Empty;
                foreach (BkgRuleMappingDetail ruleMapping in mappingToBeSaved.BkgRuleMappingDetails)
                {
                    if (!ruleMapping.IfExpressionIsvalid)
                    {
                        errorMsg += String.Format(" Please specify mapping for  \"" + ruleMapping.BRLMD_PlaceHolderName + "\".\n");
                    }
                    else if (ruleMapping.BRLMD_ConstantValue != String.Empty && ruleMapping.BRLMD_ConstantValue != null)
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
                    RuleProcessingResult xmlOutputString = validateRule(mappingToBeSaved.BkgRuleMappingDetails.ToList());
                    if (xmlOutputString.Status == 1)
                    {
                        txtValidationResult.Text = String.Format("Error: {0}", xmlOutputString.ErrorMessage);
                        btnValidate.Text = "Re-validate Rule";
                    }
                    else
                    {
                        txtValidationResult.Text = String.Format("Success: {0}", xmlOutputString.UIExpressionLabel);
                        mappingToBeSaved.BRLM_UIExpression = xmlOutputString.UIExpressionLabel;
                        if (Presenter.SaveRuleMapping(mappingToBeSaved))
                        {
                            if (CurrentViewContext.RuleMappingId == AppConsts.NONE)
                            {
                                ResetControls();
                                NotifyStatusChange("Rule saved successfully.", true);
                            }
                            else
                            {
                                EnableDisableControls(false);
                                ResetButtons(true);
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                                (this.Page as BaseWebPage).ShowSuccessMessage("Rule updated successfully.");
                            }
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

        protected void cmdBarRule_CancelClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.RuleMappingId != AppConsts.NONE)
                {
                    BindRuleInfo();
                    LoadExpressionObjects();
                    ResetButtons(true);
                    EnableDisableControls(false);
                    hdnIsCancelRequest.Value = "";
                }
                else
                {
                    ResetControls();
                    NotifyCancelClick();
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

        protected void cmdBarRule_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                ResetButtons(false);
                EnableDisableControls(true);
                cmbMasterTemplates.Enabled = false;
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

        #region Private Methods

        private void BindRuleInfo()
        {
            Presenter.GetRuleInfo();
            if (CurrentViewContext.RuleMapping != null)
            {
                CurrentViewContext.SelectedRuleTemplateId = CurrentViewContext.RuleMapping.BRLM_RuleTemplateID;
                Presenter.GetRuleTemplateDetails();
                cmbMasterTemplates.SelectedValue = Convert.ToString(CurrentViewContext.RuleMapping.BRLM_RuleTemplateID);
                txtRuleName.Text = CurrentViewContext.RuleMapping.BRLM_Name;
                txtActionMapping.Text = CurrentViewContext.RuleMapping.BRLM_ActionBlock;
                ddlRuleType.SelectedValue = Convert.ToString(CurrentViewContext.RuleMapping.BRLM_RuleType);
                ddlActionType.SelectedValue = Convert.ToString(CurrentViewContext.RuleMapping.BRLM_ActionType);
                chkActive.Checked = CurrentViewContext.RuleMapping.BRLM_IsActive;
                txtSucessMessage.Text = CurrentViewContext.RuleMapping.BRLM_SuccessMessage;
                txtErrorMessage.Text = CurrentViewContext.RuleMapping.BRLM_ErrorMessage;
                txtValidationResult.Text = CurrentViewContext.RuleMapping.BRLM_UIExpression;
                CurrentViewContext.ObjectCount = CurrentViewContext.RuleTemplateDetails.BRLT_ObjectCount;
                litExpression.Text = RuleTemplateDetails.BRLT_UIExpression;
                RuleSetId = CurrentViewContext.RuleMapping.BRLM_RuleSetID;
                //LoadExpressionObjects();
            }
        }

        private void BindTemplates()
        {
            Presenter.GetRuleTemplates();
            cmbMasterTemplates.DataSource = lstRuleTemplates;
            cmbMasterTemplates.DataTextField = "BRLT_Name";
            cmbMasterTemplates.DataValueField = "BRLT_ID";
            cmbMasterTemplates.DataBind();
            cmbMasterTemplates.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem { Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.ZERO });
        }

        private BkgRuleMapping GetRuleMapping()
        {
            BkgRuleMapping mapping = new BkgRuleMapping();
            mapping.BRLM_ID = CurrentViewContext.RuleMappingId;
            mapping.BRLM_RuleTemplateID = Convert.ToInt32(cmbMasterTemplates.SelectedValue);
            mapping.BRLM_SuccessMessage = txtSucessMessage.Text;
            mapping.BRLM_ErrorMessage = txtErrorMessage.Text;
            mapping.BRLM_RuleSetID = RuleSetId;
            mapping.BRLM_Name = txtRuleName.Text;
            mapping.BRLM_RuleType = Convert.ToInt32(ddlRuleType.SelectedValue);
            mapping.BRLM_ActionType = Convert.ToInt32(ddlActionType.SelectedValue);
            mapping.BRLM_ActionBlock = txtActionMapping.Text;
            //mapping.RLM_UIExpression = uiExpression;
            mapping.BRLM_IsDeleted = false;
            mapping.BRLM_IsActive = chkActive.Checked;
            foreach (BkgRuleMappingDetail mappingDetail in GetRuleMappingDetail())
                mapping.BkgRuleMappingDetails.Add(mappingDetail);

            return mapping;
        }

        private List<BkgRuleMappingDetail> GetRuleMappingDetail()
        {
            List<BkgRuleMappingDetail> mappingDetails = new List<BkgRuleMappingDetail>();
            List<lkpBkgObjectType> objectTypeList = Presenter.GetObjectTypeList();

            for (Int32 counter = 1; counter <= ObjectCount; counter++)
            {
                BkgRuleExpressionObject expressionObject = (BkgRuleExpressionObject)pnlExpressionObjects.FindControl(String.Format("EO_{0}", counter));
                if (expressionObject != null)
                {
                    BkgRuleMappingDetail detail = new BkgRuleMappingDetail();
                    detail.BRLMD_PlaceHolderName = expressionObject.ObjectName;
                    detail.BRLMD_CreatedByID = CurrentLoggedInUserId;
                    detail.BRLMD_CreatedOn = DateTime.Now;
                    detail.BRLMD_IsDeleted = false;
                    detail.IfExpressionIsvalid = expressionObject.IsExpressionObjectIsValid();
                    if (detail.IfExpressionIsvalid)
                    {
                        if (BkgRuleObjectMappingType.SERVICE_RESULT.GetStringValue().Equals(expressionObject.SelectedRuleObjectTypeCode))
                        {
                            detail.BRLMD_ObjectID = expressionObject.SelectedServiceId;

                            lkpBkgObjectType objectType = objectTypeList.FirstOrDefault(x => x.BOT_Code == BkgObjectType.SERVICE.GetStringValue());
                            detail.BRLMD_ObjectTypeID = objectType.BOT_ID;

                            lkpBkgRuleObjectMappingType ruleObjectMappingType = listRuleObjectMappingType.FirstOrDefault(x => x.BRMT_Code == expressionObject.SelectedRuleObjectTypeCode);
                            detail.BRLMD_RuleObjectMappingTypeID = ruleObjectMappingType.BRMT_ID;

                        }
                        else if (ObjectMappingType.Data_Value.GetStringValue().Equals(expressionObject.SelectedRuleObjectTypeCode))
                        {
                            detail.BRLMD_ObjectID = expressionObject.AttributeGrpMappingId;

                            lkpBkgObjectType objectType = objectTypeList.FirstOrDefault(x => x.BOT_Code == BkgObjectType.SERVICE_ATTRIBUTE.GetStringValue()); ;
                            detail.BRLMD_ObjectTypeID = objectType.BOT_ID;

                            lkpBkgRuleObjectMappingType ruleObjectMappingType = listRuleObjectMappingType.FirstOrDefault(x => x.BRMT_Code == expressionObject.SelectedRuleObjectTypeCode);
                            detail.BRLMD_RuleObjectMappingTypeID = ruleObjectMappingType.BRMT_ID;

                        }
                        else if (ObjectMappingType.Defined_Value.GetStringValue().Equals(expressionObject.SelectedRuleObjectTypeCode))
                        {
                            detail.BRLMD_ObjectID = null;
                            detail.BRLMD_ObjectTypeID = null;

                            lkpBkgRuleObjectMappingType ruleObjectMappingType = listRuleObjectMappingType.FirstOrDefault(x => x.BRMT_Code == expressionObject.SelectedRuleObjectTypeCode);
                            detail.BRLMD_RuleObjectMappingTypeID = ruleObjectMappingType.BRMT_ID;

                            lkpBkgConstantType constantType = ConstantTypeList.FirstOrDefault(x => x.BCT_Code == expressionObject.SelectedConstantTypeCode);
                            detail.BRLMD_ConstantType = constantType.BCT_ID;
                            detail.BRLMD_ConstantValue = expressionObject.ConstantValue;
                        }
                    }
                    mappingDetails.Add(detail);
                }
            }
            return mappingDetails;
        }

        private void ResetButtons(Boolean isVisible)
        {
            cmdBarRule.SaveButton.Visible = !isVisible;
            cmdBarRule.CancelButton.Visible = !isVisible;
            cmdBarRule.SubmitButton.Visible = isVisible;
            if (!isVisible)
            {
                cmdBarRule.SaveButton.ValidationGroup = "grpSubmit";
                if (CurrentViewContext.RuleMappingId != AppConsts.NONE)
                {
                    cmdBarRule.SaveButton.Text = "Update";
                }
                else
                {
                    cmdBarRule.SaveButton.Text = "Save";
                }
            }
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

        private void EnableDisableControls(Boolean isEnable)
        {
            txtRuleName.Enabled = isEnable;
            ddlActionType.Enabled = isEnable;
            ddlRuleType.Enabled = isEnable;
            txtActionMapping.Enabled = isEnable;
            chkActive.IsActiveEnable = isEnable;
            txtSucessMessage.Enabled = isEnable;
            txtErrorMessage.Enabled = isEnable;
            txtActionMapping.Enabled = isEnable;
            cmbMasterTemplates.Enabled = isEnable;
            //litExpression.Enabled = isEnable;
            txtValidationResult.Enabled = isEnable;
            pnlExpressionObjects.Enabled = isEnable;
            btnValidate.Enabled = isEnable;
        }

        private void LoadExpressionObjects()
        {
            if (CurrentViewContext.ObjectCount > 0)
            {
                if (CurrentViewContext.RuleMapping == null)
                {
                    for (int rowId = 1; rowId <= ObjectCount; rowId++)
                    {
                        BkgRuleExpressionObject ctrl = (BkgRuleExpressionObject)LoadControl("~/BkgSetup/UserControl/BkgRuleExpressionObject.ascx");
                        ctrl.RowId = rowId;
                        ctrl.ID = String.Format("EO_{0}", rowId);
                        ctrl.PackageId = PackageId;
                        ctrl.SelectedTenantId = SelectedTenantId;
                        ctrl.bkgDataStore = BackgroundDataStore;
                        ctrl.lstRuleObjectMappingType = listRuleObjectMappingType;
                        pnlExpressionObjects.Controls.Add(ctrl);
                    }
                    return;
                }
                Int32 count = 1;
                foreach (BkgRuleMappingDetail detail in CurrentViewContext.RuleMapping.BkgRuleMappingDetails.OrderBy(x => { return Convert.ToInt32(x.BRLMD_PlaceHolderName.Replace("[Object", "").Replace("]", "")); }))
                {
                    if (detail != null)
                    {
                        String ruleMappingObjectTypeCode = String.Empty;
                        String selectedObjectTypeCode = String.Empty;
                        Int32? selectedServiceId = new Int32?();
                        Int32? attribteGroupMappingId = new Int32?();
                        String constantValue = String.Empty;
                        Int32? constantTypeId = new Int32?();
                        if (detail.BRLMD_RuleObjectMappingTypeID != null)
                        {
                            ruleMappingObjectTypeCode = listRuleObjectMappingType.FirstOrDefault(x => x.BRMT_ID == detail.lkpBkgRuleObjectMappingType.BRMT_ID).BRMT_Code;
                            if (BkgRuleObjectMappingType.SERVICE_RESULT.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                selectedObjectTypeCode = detail.lkpBkgObjectType.BOT_Code;
                                selectedServiceId = detail.BRLMD_ObjectID;
                            }

                            if (ObjectMappingType.Data_Value.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                selectedObjectTypeCode = detail.lkpBkgObjectType.BOT_Code;
                                attribteGroupMappingId = detail.BRLMD_ObjectID;
                            }

                            if (ObjectMappingType.Defined_Value.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                constantValue = detail.BRLMD_ConstantValue;
                                if (detail.BRLMD_ConstantType != null)
                                {
                                    constantTypeId = detail.BRLMD_ConstantType;
                                }
                            }
                        }
                        BkgRuleExpressionObject expressionObject = (BkgRuleExpressionObject)LoadControl("~/BkgSetup/UserControl/BkgRuleExpressionObject.ascx");

                        expressionObject.RowId = count;
                        expressionObject.ID = String.Format("EO_{0}", count);
                        expressionObject.PackageId = PackageId;
                        expressionObject.SelectedTenantId = SelectedTenantId;
                        expressionObject.ObjectName = detail.BRLMD_PlaceHolderName;
                        expressionObject.SelectedRuleObjectTypeCode = ruleMappingObjectTypeCode;
                        // expressionObject.objectTypeCode = selectedObjectTypeCode;
                        pnlExpressionObjects.Controls.Add(expressionObject);
                        if (selectedServiceId.HasValue)
                            expressionObject.SelectedServiceId = selectedServiceId.Value;
                        if (attribteGroupMappingId.HasValue)
                            expressionObject.AttributeGrpMappingId = attribteGroupMappingId.Value;
                        expressionObject.ConstantValue = constantValue;
                        expressionObject.constantTypeId = constantTypeId;
                        expressionObject.bkgDataStore = BackgroundDataStore;
                        expressionObject.lstRuleObjectMappingType = listRuleObjectMappingType;
                        count++;
                    }
                }
            }
        }

        #endregion


        #region Validate Rule
        /// <summary>
        /// Method used for validating rule
        /// </summary>
        /// <returns>Rule Processing Result</returns>
        private RuleProcessingResult validateRule(List<BkgRuleMappingDetail> ruleMappingList)
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
        private String generateMappingXml(List<BkgRuleMappingDetail> mappingDetails)
        {
            List<Int32> attribteGrpMappingId = new List<Int32>();
            List<ComplianceAttribute> attributeDetail = new List<ComplianceAttribute>();
            foreach (BkgRuleMappingDetail detail in mappingDetails)
            {
                if (detail.BRLMD_RuleObjectMappingTypeID.IsNull())
                {
                    continue;
                }
                if (listRuleObjectMappingType.FirstOrDefault(x => x.BRMT_ID == detail.BRLMD_RuleObjectMappingTypeID.Value).BRMT_Code == "DVAL")
                {
                    attribteGrpMappingId.Add(detail.BRLMD_ObjectID.Value);
                }
            }
            if (attribteGrpMappingId.Count > 0)
            {
                // attributeDetail = Presenter.getAttributeDetail(objectIds);
            }
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("ObjectMapping"));
            XmlNode expGroup = el.AppendChild(doc.CreateElement("Mappings"));

            //for intializing object counter
            Int32 counter = 1;
            foreach (BkgRuleMappingDetail detail in mappingDetails)
            {
                BkgRuleExpressionObject expressionObject = (BkgRuleExpressionObject)pnlExpressionObjects.FindControl(String.Format("EO_{0}", counter));

                if (detail.BRLMD_RuleObjectMappingTypeID.IsNull())
                {
                    continue;
                }
                XmlNode exp = expGroup.AppendChild(doc.CreateElement("Mapping"));
                exp.AppendChild(doc.CreateElement("Key")).InnerText = detail.BRLMD_PlaceHolderName;
                if (expressionObject.SelectedRuleObjectTypeCode == "SRVC")
                {
                    exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.Boolean.GetStringValue();
                    exp.AppendChild(doc.CreateElement("MappedName")).InnerText = expressionObject.SelectedServiceName.ToString();
                }
                else if (expressionObject.SelectedRuleObjectTypeCode == "CONST")
                {
                    //when current Day is selected as a constant
                    if (expressionObject.SelectedConstantTypeCode == BkgConstantType.DAY.GetStringValue() && expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.Date.GetStringValue();
                    }

                    //when current Month or Year is selected as a constant
                    else if ((expressionObject.SelectedConstantTypeCode == BkgConstantType.MONTH.GetStringValue()
                         || expressionObject.SelectedConstantTypeCode == BkgConstantType.YEAR.GetStringValue())
                        && expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = BkgConstantType.NUMEIC.GetStringValue();
                    }

                    //when Empty is selected as a constant
                    else if (expressionObject.ConstantValue == AppConsts.EMPTY)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.Text.GetStringValue();
                    }

                    //when manual Day is selected as a constant
                    else if (expressionObject.SelectedConstantTypeCode == BkgConstantType.DAY.GetStringValue() && !expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.TimeSpanDay.GetStringValue();
                    }

                    //when manual mnth is selected as a constant
                    else if (expressionObject.SelectedConstantTypeCode == BkgConstantType.MONTH.GetStringValue() && !expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.TimeSpanMnth.GetStringValue();
                    }

                     //when manual Year is selected as a constant 
                    else if (expressionObject.SelectedConstantTypeCode == BkgConstantType.YEAR.GetStringValue() && !expressionObject.CurrentChecked)
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = OperandType.TimeSpanYear.GetStringValue();
                    }

                    else
                    {
                        exp.AppendChild(doc.CreateElement("DataType")).InnerText = GetOperandType(detail.BRLMD_ConstantValue).GetStringValue();
                    }
                    exp.AppendChild(doc.CreateElement("MappedName")).InnerText = expressionObject.ConstantValue;
                }
                else if (expressionObject.SelectedRuleObjectTypeCode == "DVAL")
                {
                    String datatype = String.Empty;
                    var selectedAttribute = expressionObject.attributeList.Where(obj => obj.BkgAttributeGroupMappingId == detail.BRLMD_ObjectID).FirstOrDefault();
                    if (selectedAttribute != null)
                        datatype = selectedAttribute.AttributeType;
                    if (datatype == OperandType.Country.GetStringValue() || datatype == OperandType.County.GetStringValue()
                        || datatype == OperandType.State.GetStringValue() || datatype == OperandType.City.GetStringValue()
                        || datatype == OperandType.ZipCode.GetStringValue() || datatype == OperandType.Cascading.GetStringValue() ) //UAT 3541
                    {
                        datatype = OperandType.Text.GetStringValue();
                    }
                    else if (datatype == "Option")
                    {
                        datatype = OperandType.Text.GetStringValue();
                    }
                    exp.AppendChild(doc.CreateElement("DataType")).InnerText = datatype;
                    exp.AppendChild(doc.CreateElement("MappedName")).InnerText = expressionObject.SelectedAttributeName;
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
            el.AppendChild(doc.CreateElement("ResultType")).InnerText = RuleTemplateDetails.lkpBkgRuleResultType.BRSL_Code;
            XmlNode expGroup = el.AppendChild(doc.CreateElement("Expressions"));

            //UAT-2684:Fix "Mapping not provided for token E#" issue
            List<BkgRuleTemplateExpression> lstRuleTemplateExpression = RuleTemplateDetails.BkgRuleTemplateExpressions.Where(x => !x.BRLE_IsDeleted).OrderBy(ord => ord.BRLE_ExpressionOrder).ToList();
            foreach (var expression in lstRuleTemplateExpression)
            {
                XmlNode exp = expGroup.AppendChild(doc.CreateElement("Expression"));

                string expressionName = expression.BkgExpression.BEX_Name == "(Group)" ? "GE" : expression.BkgExpression.BEX_Name;
                exp.AppendChild(doc.CreateElement("Name")).InnerText = expressionName;

                string definition = expression.BkgExpression.BEX_Expression;
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

        private String validateConstantInput(BkgRuleMappingDetail ruleMapping)
        {
            String errorMsg = String.Empty;

            lkpBkgConstantType constantType = ConstantTypeList.FirstOrDefault(x => x.BCT_ID == ruleMapping.BRLMD_ConstantType);
            if (constantType.BCT_Code != String.Empty)
            {
                if (ruleMapping.BRLMD_ConstantValue == AppConsts.CURRENT_DAY || ruleMapping.BRLMD_ConstantValue == AppConsts.CURRENT_MONTH
                    || ruleMapping.BRLMD_ConstantValue == AppConsts.CURRENT_YEAR || ruleMapping.BRLMD_ConstantValue == AppConsts.EMPTY
                    || constantType.BCT_Code == BkgConstantType.COUNTRY.GetStringValue() || constantType.BCT_Code == BkgConstantType.COUNTY.GetStringValue()
                    || constantType.BCT_Code == BkgConstantType.STATE.GetStringValue())
                {
                    return errorMsg;
                }
                else
                {
                    if (constantType.BCT_Code == BkgConstantType.BOOL.GetStringValue())
                    {
                        try
                        {
                            Boolean tempD = Boolean.Parse(ruleMapping.BRLMD_ConstantValue);
                        }
                        catch
                        {
                            errorMsg += String.Format("\"" + ruleMapping.BRLMD_ConstantValue + "\" is not an appropriate value for constant type \"" + constantType.BCT_Name + "\".\n");
                        }
                    }
                    else if (constantType.BCT_Code == BkgConstantType.NUMEIC.GetStringValue())
                    {
                        try
                        {
                            Decimal tempD = Decimal.Parse(ruleMapping.BRLMD_ConstantValue);
                        }
                        catch
                        {
                            errorMsg += String.Format("\"" + ruleMapping.BRLMD_ConstantValue + "\" is not an appropriate value for constant type \"" + constantType.BCT_Name + "\".\n");
                        }

                    }
                    else if (constantType.BCT_Code == BkgConstantType.DATE.GetStringValue())
                    {
                        try
                        {
                            DateTime tempD = DateTime.Parse(ruleMapping.BRLMD_ConstantValue);
                        }
                        catch
                        {
                            errorMsg += String.Format("\"" + ruleMapping.BRLMD_ConstantValue + "\" is not an appropriate value for constant type \"" + constantType.BCT_Name + "\".\n");
                        }
                    }
                    else if (constantType.BCT_Code == BkgConstantType.DAY.GetStringValue()
                        || constantType.BCT_Code == BkgConstantType.MONTH.GetStringValue()
                        || constantType.BCT_Code == BkgConstantType.YEAR.GetStringValue())
                    {
                        try
                        {
                            Int32 tempD = Int32.Parse(ruleMapping.BRLMD_ConstantValue);
                        }
                        catch
                        {
                            errorMsg += String.Format("\"" + ruleMapping.BRLMD_ConstantValue + "\" is not an appropriate value for constant type \"" + constantType.BCT_Name + "\".\n");
                        }
                    }
                }
                return errorMsg;
            }
            return errorMsg;
        }
        #endregion

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

                                if (x.FeatureAction.CustomActionId == "EditRule")
                                {
                                    cmdBarRule.SubmitButton.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "EditRule")
                                {
                                    cmdBarRule.SubmitButton.Visible = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }

    }
}