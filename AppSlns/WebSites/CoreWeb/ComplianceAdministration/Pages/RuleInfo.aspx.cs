using System;
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
    public partial class RuleInfo : System.Web.UI.Page, IRuleInfoView
    {
        #region Variable

        #region Private Variables
        private RuleInfoPresenter _presenter = new RuleInfoPresenter();
        Int32 tenantid = 0;
        private ComplianceDataStore _complinceDataStore;
        private List<lkpRuleObjectMappingType> _listRuleObjectMappingType;
        private List<lkpConstantType> _constantTypeList;
        private String _settingXml = String.Empty;
        private Boolean _isScheduleActionRecordInserted = false;
        #endregion

        #region Public variables

        #endregion
        #endregion


        #region Properties

        public RuleInfoPresenter Presenter
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

        public IRuleInfoView CurrentViewContext
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

        public Boolean IsAllSelected
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAllSelected"]);
            }
            set
            {
                ViewState["IsAllSelected"] = value;
            }
        }

        public Boolean IsNewSelected
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsNewSelected"]);
            }
            set
            {
                ViewState["IsNewSelected"] = value;
            }
        }

        public Boolean IsExistingSelected
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsExistingSelected"]);
            }
            set
            {
                ViewState["IsExistingSelected"] = value;
            }
        }

        public List<UserGroupContract> UserGroups
        {
            get
            {
                if (!(ViewState["UserGroups"] is List<UserGroupContract>))
                {
                    ViewState["UserGroups"] = new List<UserGroupContract>();
                }
                return (List<UserGroupContract>)ViewState["UserGroups"];
            }
            set
            {
                ViewState["UserGroups"] = value;
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

        public Boolean? IfUpdateAllIsSelected
        {
            get;
            set;
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
        //UAT-2147 Auto-fill in dropdowns on rule mapping on tracking side
        Int32 IRuleInfoView.CurrentCategoryID
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentCategoryID"]);
            }
            set
            {
                ViewState["ParentCategoryID"] = value;
            }
        }
        #endregion


        #region Events
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["PackageId"] != null)
                PackageId = Convert.ToInt32(Request.QueryString["PackageId"]);

            if (Request.QueryString["Id"] != null)
                if (CurrentViewContext.RuleMappingId == null || CurrentViewContext.RuleMappingId == AppConsts.NONE)
                {
                    CurrentViewContext.RuleMappingId = Convert.ToInt32(Request.QueryString["Id"]);
                }

            if (Request.QueryString["SelectedTenantId"] != null)
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
            //UAT-2147 Get the Current Category ID 
            if (Request.QueryString["CurrentCategoryID"] != null)
            {
                CurrentViewContext.CurrentCategoryID = Convert.ToInt32(Request.QueryString["CurrentCategoryID"]);
            }

            hdnSelectedTenantID.Value = CurrentViewContext.SelectedTenantId.ToString();
            hdnRuleMappingID.Value = CurrentViewContext.RuleMappingId.ToString();
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                IfAnySubscriptionExist = Presenter.IsAnySubscriptionExist(PackageId);
                BindRuleInfo();
                ResetButtons(true);
                SetCreateNewVersionButton();
                SetFormMode(false);
                BindSharedInstances();
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

        #endregion

        protected void fsucCmdBarRule_CancelClick(object sender, EventArgs e)
        {
            lblSuccess.Visible = false;
            lblSuccess.Text = string.Empty;
            BindRuleInfo();
            ResetButtons(true);
            SetFormMode(false);
            LoadExpressionObjects();
            SetCreateNewVersionButton();
            rbUpdateOption.SelectedIndex = -1;
            hdnIsCancelRequest.Value = "";
        }

        protected void fsucCmdBarRule_SubmitClick(object sender, EventArgs e)
        {
            lblSuccess.Visible = false;
            BindRuleInfo();
            ResetButtons(false);
            SetFormMode(true);
            CurrentViewContext.IsVersionUpdate = false;
            fsucCmdBarRule.SaveButton.ValidationGroup = "grpFormSubmit";
        }

        protected void fsucCmdBarRule_SaveClick(object sender, EventArgs e)
        {
            RuleMapping mappingToBeSaved = GetRuleMapping();
            String errorMsg = String.Empty;
            List<Int32> pkgIDsSharingRuleInstance = new List<Int32>();
            List<Int32> lstCategoryMappedinObjects = new List<Int32>();
            List<Tuple<Int32, Int32, Int32>> objectMappings = new List<Tuple<Int32, Int32, Int32>>();
            string catObjectType = INTSOF.Utils.ObjectType.Compliance_Category.GetStringValue();
            List<lkpObjectType> objectTypeList = Presenter.GetObjectTypeList();
            lkpObjectType objectTypeCat = objectTypeList.FirstOrDefault(x => x.OT_Code.Equals(catObjectType));
            int ruleSetID = mappingToBeSaved.RLM_RuleSetID;

            AssignmentHierarchy objAssignmentHierarchy = new AssignmentHierarchy();
            objAssignmentHierarchy = Presenter.GetAssignmentHierarchyByRuleSetId(ruleSetID);

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

                if (ruleMapping.RLMD_ConstantValue.IsNullOrEmpty() && !ruleMapping.RLMD_ObjectID.IsNullOrEmpty())
                {
                    if (ruleMapping.RuleMappingObjectTrees.Where(con => con.RMOT_RuleSetTreeID == objectTypeCat.OT_ID && con.RMOT_ObjectID != CurrentViewContext.CurrentCategoryID).Any())
                    {
                        Int32 categoryObjectID = ruleMapping.RuleMappingObjectTrees.Where(con => con.RMOT_RuleSetTreeID == objectTypeCat.OT_ID && con.RMOT_ObjectID != CurrentViewContext.CurrentCategoryID).FirstOrDefault().RMOT_ObjectID;
                        if (categoryObjectID != 0)
                        {
                            lstCategoryMappedinObjects.Add(categoryObjectID);
                            objectMappings.Add(new Tuple<Int32, Int32, Int32>(categoryObjectID, ruleMapping.RLMD_ObjectTypeID.Value, ruleMapping.RLMD_ObjectID.Value));
                        }
                    }
                }

                //// Retrive category Ids here from Rule MappingDetails
                //foreach (RuleMappingObjectTree objRuleMappingObjectTree in ruleMapping.RuleMappingObjectTrees)
                //{
                //    if (objRuleMappingObjectTree.RMOT_RuleSetTreeID == 2 && objRuleMappingObjectTree.RMOT_ObjectID != CurrentViewContext.CurrentCategoryID)
                //        categoryIdList.Add(objRuleMappingObjectTree.RMOT_ObjectID);
                //}
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
                if (rbUpdateOption.Visible && Convert.ToInt32(rbUpdateOption.SelectedValue) == AppConsts.ONE)
                {
                    List<CompliancePackage> packageListSharingRuleInstance = Presenter.GetListOfInstanceWhichCanShareRule(ruleSetID, objAssignmentHierarchy.HID);
                    foreach (CompliancePackage comPkg in packageListSharingRuleInstance)
                    {
                        pkgIDsSharingRuleInstance.Add(Convert.ToInt32(comPkg.CompliancePackageID));                        
                    }
                }
                if (cmbShareBetween.Visible && cmbShareBetween.CheckedItems.Count > 0)
                {
                    foreach (var checkedItem in cmbShareBetween.CheckedItems)
                    {
                        pkgIDsSharingRuleInstance.Add(Convert.ToInt32(checkedItem.Value));                        
                    }                    
                }
                if (!pkgIDsSharingRuleInstance.IsNullOrEmpty())
                {

                    Dictionary<String, String> unMappedObjects = Presenter.IsCategoriesAvailableinSelectedPackages(pkgIDsSharingRuleInstance.Distinct().ToList(), lstCategoryMappedinObjects, objectMappings);

                    if (unMappedObjects.Count == 0)
                    {
                        ContinueToSaveRule(mappingToBeSaved);
                    }
                    else
                    {
                        lblSuccess.Visible = true;
                        String _errorMsg = string.Empty;
                        if (unMappedObjects.Count == 1)
                        {
                            foreach (KeyValuePair<string, string> dictItem in unMappedObjects)
                            {
                                _errorMsg = dictItem.Value + " does not exist in " + dictItem.Key;
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<string, string> dictItem in unMappedObjects)
                            {
                                if (_errorMsg == String.Empty)
                                {
                                    _errorMsg = dictItem.Value + " does not exist in " + dictItem.Key;
                                }
                                else
                                {
                                    _errorMsg = _errorMsg + ", " + dictItem.Value + " does not exist in " + dictItem.Key;
                                }

                            }
                        }

                        lblSuccess.ShowMessage("Rule Cannot be mapped because " + _errorMsg + ".", MessageType.Error);

                        //lblSuccess.ShowMessage("Rule Cannot be mapped because " + unMappedRuleObjectItemList + " could not be mapped as it does not exist in " + addToPkgNames + " package(s).", MessageType.Error);
                    }
                }
                else
                {
                    ContinueToSaveRule(mappingToBeSaved);
                }
            }
        }

        private void ContinueToSaveRule(RuleMapping mappingToBeSaved)
        {
            CurrentViewContext.IsAllSelected = cmbUserGroup.CheckedItems.Any(x => x.Value == String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.All)));
            CurrentViewContext.IsNewSelected = cmbUserGroup.CheckedItems.Any(x => x.Value == String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.New)));
            CurrentViewContext.IsExistingSelected = cmbUserGroup.CheckedItems.Any(x => x.Value == String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.Existing)));
            RuleProcessingResult xmlOutputString = validateRule(mappingToBeSaved.RuleMappingDetails.ToList());
            if (xmlOutputString.Status == 1)
            {
                txtValidationResult.Text = String.Format("Error: {0}", xmlOutputString.ErrorMessage);
                btnValidate.Text = "Re-validate Rule";
            }
            else
            {
                //Added new  logic for editablity of rule 
                if (IsAllSelected || IsNewSelected || !IfAnySubscriptionExist) //first two conditions are according to flow chart and third condition is assumption
                {
                    mappingToBeSaved.RLM_IsCurrent = true;
                }
                else
                {
                    mappingToBeSaved.RLM_IsCurrent = false;
                }
                mappingToBeSaved.RLM_UIExpression = xmlOutputString.UIExpressionLabel;
                if (IsVersionUpdate)
                {
                    Presenter.SaveRuleMapping(mappingToBeSaved);
                    assignRuleToUserGroups();
                }
                else
                {
                    Presenter.UpdateRuleMapping(mappingToBeSaved);
                    if (IfAnySubscriptionExist)
                    {
                        assignRuleToUserGroups();
                    }
                }

                if ((rbUpdateOption.SelectedIndex > -1 || cmbShareBetween.CheckedItems.Count > 0) && (!Presenter.IsDefaultTenant()))
                {
                    List<Int32> packagelist = new List<Int32>();
                    foreach (var checkedItem in cmbShareBetween.CheckedItems)
                    {
                        packagelist.Add(Convert.ToInt32(checkedItem.Value));
                    }
                    if (rbUpdateOption.SelectedIndex == 0)
                    {
                        IfUpdateAllIsSelected = false;
                    }
                    else if (rbUpdateOption.SelectedIndex == 1)
                    {
                        IfUpdateAllIsSelected = true;
                    }
                    Presenter.ComplianceRuleSynchronisationonRuleEdit(packagelist);
                }

                if (IsScheduleActionRecordInserted)
                {
                    Presenter.InsertSystemServiceTrigger();
                }

                String data = String.Empty;
                if (IsVersionUpdate)
                {
                    data = String.Format("{{\"DataId\":\"{0}\",\"ParentDataId\":\"{1}\",\"UICode\":\"{2}\"}}", RuleMappingId, RuleSetId, RuleSetTreeNodeType.Rule);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree('" + data + "');", true);
                BindRuleInfo();
                ResetButtons(true);
                SetFormMode(false);
                lblSuccess.Visible = true;
                BindSharedInstances();
                if (ErrMsg == String.Empty)
                {
                    if (IsVersionUpdate)
                    {
                        lblSuccess.ShowMessage("Rule version created successfully.", MessageType.SuccessMessage);
                    }
                    else
                    {
                        lblSuccess.ShowMessage("Rule updated successfully.", MessageType.SuccessMessage);
                    }
                }
                else
                    lblSuccess.ShowMessage(ErrMsg, MessageType.Information);
                SetCreateNewVersionButton();
            }
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

        protected void fsucCmdBarRule_ClearClick(object sender, EventArgs e)
        {
            lblSuccess.Visible = false;
            CurrentViewContext.IsVersionUpdate = true;
            BindRuleInfo();
            ResetButtons(false);
            SetFormMode(true);
            fsucCmdBarRule.SaveButton.ValidationGroup = "grpFormSubmit";
        }

        protected void cmbMasterTemplates_SelectedIndexChanged(object sender, EventArgs e)
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
            hdnRuleActionType.Value = Convert.ToString(CurrentViewContext.RuleTemplateDetails.lkpRuleActionType.ACT_ID); //UAT-2740
            litExpression.Text = RuleTemplateDetails.RLT_UIExpression;

            LoadExpressionObjects();
            txtValidationResult.Text = String.Empty;
        }
        #endregion



        #region Methods
        #region Private Methods

        private void BindRuleInfo()
        {
            hdnRuleMappingID.Value = CurrentViewContext.RuleMappingId.ToString();
            Presenter.getRuleInfo();
            if (CurrentViewContext.RuleMapping != null)
            {
                Presenter.GetRuleTemplates();
                BindTemplates();
                cmbMasterTemplates.SelectedValue = Convert.ToString(CurrentViewContext.RuleMapping.RLM_RuleTemplateID);
                txtRuleName.Text = CurrentViewContext.RuleMapping.RLM_Name;
                txtActionMapping.Text = CurrentViewContext.RuleMapping.RLM_ActionBlock;
                txtSucessMessage.Text = CurrentViewContext.RuleMapping.RLM_SuccessMessage;
                if (IsVersionUpdate)
                {
                    chkActive.Checked = true;
                }
                else
                {
                    chkActive.Checked = CurrentViewContext.RuleMapping.RLM_IsActive;
                }
                txtErrorMessage.Text = CurrentViewContext.RuleMapping.RLM_ErrorMessage;
                txtValidationResult.Text = CurrentViewContext.RuleMapping.RLM_UIExpression;
                CurrentViewContext.SelectedRuleTemplateId = CurrentViewContext.RuleMapping.RLM_RuleTemplateID;

                Presenter.GetRuleTemplateDetails();
                CurrentViewContext.ObjectCount = CurrentViewContext.RuleTemplateDetails.RLT_ObjectCount;
                lblAction.Text = CurrentViewContext.RuleTemplateDetails.lkpRuleActionType.ACT_Description;
                hdnRuleActionType.Value = Convert.ToString(CurrentViewContext.RuleTemplateDetails.lkpRuleActionType.ACT_ID); //UAT-2740
                litExpression.Text = RuleTemplateDetails.RLT_UIExpression;
                if (IfAnySubscriptionExist)
                {
                    //cmbUserGroup.Visible = true;
                    dvUsergroup.Visible = true;
                    bindUserGroup();
                    rfvUserGroup.Enabled = true;
                }
                else
                {
                    dvUsergroup.Visible = false;
                    rfvUserGroup.Enabled = false;
                }
                CurrentViewContext.FirstVersionRuleId = CurrentViewContext.RuleMapping.RLM_FirstVersionID.HasValue ? CurrentViewContext.RuleMapping.RLM_FirstVersionID : null;
                RuleSetId = CurrentViewContext.RuleMapping.RLM_RuleSetID;
                //LoadExpressionObjects();
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
                        ExpressionObject ctrl = (ExpressionObject)LoadControl("~/ComplianceAdministration/UserControl/ExpressionObject.ascx");
                        ctrl.RowId = rowId;
                        ctrl.ID = String.Format("EO_{0}", rowId);
                        ctrl.PackageId = PackageId;
                        ctrl.SelectedTenantId = SelectedTenantId;
                        ctrl.complinceDataStore = complinceDataStore;
                        ctrl.lstRuleObjectMappingType = listRuleObjectMappingType;
                        ctrl.CurrentCategoryID = CurrentViewContext.CurrentCategoryID;
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
                        Int32? SelectedItemSubmissionDateID = new Int32?();
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

                            if (ObjectMappingType.Data_Value.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                selectedObjectTypeCode = detail.lkpObjectType.OT_Code;
                                //RuleMappingObjectTree ruleMappingObjectTreeForCategory = GetRuleMappingObjectTreeDetail(detail.RLMD_ID, RuleSetTreeType.Categories);
                                //RuleMappingObjectTree ruleMappingObjectTreeForItem = GetRuleMappingObjectTreeDetail(detail.RLMD_ID, RuleSetTreeType.Items);
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

                            if (ObjectMappingType.Defined_Value.GetStringValue().Equals(ruleMappingObjectTypeCode))
                            {
                                constantValue = detail.RLMD_ConstantValue;
                                if (detail.RLMD_ConstantType != null)
                                {
                                    constantTypeId = detail.RLMD_ConstantType;
                                }
                                if (detail.lkpConstantType.Code == ConstantType.ItemSubmissionDate.GetStringValue())
                                {
                                    SelectedItemSubmissionDateID = detail.RLMD_ObjectID.Value;
                                }

                            }
                        }
                        object[] parameters = new object[] { ruleMappingObjectTypeCode, selectedObjectTypeCode, selectedCategoryId, selectedItemId, selectedAttributeId };
                        ExpressionObject expressionObject = (ExpressionObject)LoadControl("~/ComplianceAdministration/UserControl/ExpressionObject.ascx");

                        expressionObject.RowId = count;
                        expressionObject.ID = String.Format("EO_{0}", count);
                        expressionObject.PackageId = PackageId;
                        expressionObject.SelectedTenantId = SelectedTenantId;
                        expressionObject.ObjectName = detail.RLMD_PlaceHolderName;
                        expressionObject.SelectedMappingTypeCode = ruleMappingObjectTypeCode;
                        expressionObject.objectTypeCode = selectedObjectTypeCode;
                        expressionObject.categoryId = selectedCategoryId;
                        expressionObject.itemId = selectedItemId;
                        expressionObject.attributeId = selectedAttributeId;
                        pnlExpressionObjects.Controls.Add(expressionObject);
                        expressionObject.constantTypeId = constantTypeId;
                        expressionObject.ConstantValue = constantValue;
                        expressionObject.complinceDataStore = complinceDataStore;
                        expressionObject.CurrentCategoryID = CurrentViewContext.CurrentCategoryID;
                        expressionObject.lstRuleObjectMappingType = listRuleObjectMappingType;
                        expressionObject.SelectedItemSubmissionDateID = Convert.ToString(SelectedItemSubmissionDateID);
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
                ExpressionObject expressionObject = (ExpressionObject)pnlExpressionObjects.FindControl(String.Format("EO_{0}", counter));
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

                            RuleMappingObjectTree ruleMappingObjectTreeForPackage = GetRuleMappingObjectTree(PackageId, RuleSetTreeType.Packages, ruleSetTree);
                            if (ruleMappingObjectTreeForPackage != null)
                                detail.RuleMappingObjectTrees.Add(ruleMappingObjectTreeForPackage);

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

                            RuleMappingObjectTree ruleMappingObjectTreeForPackage = GetRuleMappingObjectTree(PackageId, RuleSetTreeType.Packages, ruleSetTree);
                            if (ruleMappingObjectTreeForPackage != null)
                                detail.RuleMappingObjectTrees.Add(ruleMappingObjectTreeForPackage);

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
                            if (expressionObject.selectedConstantTypeCode == ConstantType.ItemSubmissionDate.GetStringValue())
                            {
                                detail.RLMD_ObjectID = Convert.ToInt32(expressionObject.SelectedItemSubmissionDateID);// Convert.ToInt32(expressionObject.ConstantValue);

                                lkpObjectType objectType = objectTypeList.FirstOrDefault(x => x.OT_Code == ObjectType.Compliance_Item.GetStringValue());
                                detail.RLMD_ObjectTypeID = objectType.OT_ID;

                                RuleMappingObjectTree ruleMappingObjectTreeForPackage = GetRuleMappingObjectTree(PackageId, RuleSetTreeType.Packages, ruleSetTree);
                                if (ruleMappingObjectTreeForPackage != null)
                                    detail.RuleMappingObjectTrees.Add(ruleMappingObjectTreeForPackage);

                                RuleMappingObjectTree ruleMappingObjectTreeForCategory = GetRuleMappingObjectTree(Convert.ToInt32(expressionObject.CurrentCategoryID), RuleSetTreeType.Categories, ruleSetTree);
                                if (ruleMappingObjectTreeForCategory != null)
                                    detail.RuleMappingObjectTrees.Add(ruleMappingObjectTreeForCategory);
                            }
                            detail.RLMD_ConstantType = constantType.ID;
                            detail.RLMD_ConstantValue = expressionObject.ConstantValue;
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
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("ObjectMapping"));
            XmlNode expGroup = el.AppendChild(doc.CreateElement("Mappings"));

            //for intializing object counter
            Int32 counter = 1;
            foreach (RuleMappingDetail detail in mappingDetails)
            {
                ExpressionObject expressionObject = (ExpressionObject)pnlExpressionObjects.FindControl(String.Format("EO_{0}", counter));

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
                    //When ItemSubmissionDate Selected as a Constant
                    else if (expressionObject.selectedConstantTypeCode == ConstantType.ItemSubmissionDate.GetStringValue())
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
                    if (datatype.ToLower() == "signature")
                    {
                        datatype = OperandType.Boolean.GetStringValue();
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
            if (!IsVersionUpdate)
            {
                cmbMasterTemplates.Enabled = false;
            }
            else
            {
                cmbMasterTemplates.Enabled = isEnabled;
            }
            txtRuleName.Enabled = isEnabled;
            lblAction.Enabled = isEnabled;
            txtActionMapping.Enabled = isEnabled;
            if (!IsVersionUpdate)
            {
                chkActive.IsActiveEnable = isEnabled;
            }
            else
            {
                chkActive.IsActiveEnable = false;
            }
            txtErrorMessage.Enabled = isEnabled;
            txtSucessMessage.Enabled = isEnabled;
            pnlExpressionObjects.Enabled = isEnabled;
            btnValidate.Enabled = isEnabled;
            txtValidationResult.Enabled = isEnabled;
            ddlTestUsersList.Enabled = isEnabled;
            txtTestResult.Enabled = isEnabled;
            cmbUserGroup.Enabled = isEnabled;
            cmbShareBetween.Enabled = isEnabled;
            rbUpdateOption.Enabled = isEnabled;
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

        private void bindUserGroup()
        {
            //cmbUserGroup.CheckedItems.Clear();
            List<UserGroup> definedUserGroup = Presenter.getAllUsergroups();
            if (UserGroups.Count == AppConsts.NONE)
            {
                List<UserGroupContract> userGroupList = UserGroups;
                userGroupList.Add(new UserGroupContract()
                {
                    UserGroupDataId = String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.All)),
                    UserGroupId = null,
                    UserGroupName = FixedUserGroups.All.GetStringValue(),
                    UserGroupType = UserGroupType.Fixed.GetStringValue()
                });

                userGroupList.Add(new UserGroupContract()
                {
                    UserGroupDataId = String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.New)),
                    UserGroupId = null,
                    UserGroupName = FixedUserGroups.New.GetStringValue(),
                    UserGroupType = UserGroupType.Fixed.GetStringValue()
                });

                userGroupList.Add(new UserGroupContract()
                {
                    UserGroupDataId = String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.Existing)),
                    UserGroupId = null,
                    UserGroupName = FixedUserGroups.Existing.GetStringValue(),
                    UserGroupType = UserGroupType.Fixed.GetStringValue()
                });

                if (definedUserGroup != null)
                {
                    foreach (UserGroup userGroup in definedUserGroup)
                    {
                        userGroupList.Add(new UserGroupContract()
                        {
                            UserGroupId = userGroup.UG_ID,
                            UserGroupDataId = String.Concat(UserGroupType.Defined.GetStringValue(), userGroup.UG_ID),
                            UserGroupName = userGroup.UG_Name,
                            UserGroupType = UserGroupType.Defined.GetStringValue()
                        });
                    }
                }
                UserGroups = userGroupList;
            }
            cmbUserGroup.DataSource = UserGroups;
            cmbUserGroup.DataTextField = "UserGroupName";
            cmbUserGroup.DataValueField = "UserGroupDataId";
            cmbUserGroup.DataBind();
            CheckedPreviouslyMappedUserGroups();
        }

        private void CheckedPreviouslyMappedUserGroups()
        {
            List<RuleImpactGroupMapping> previousMappedGroups = Presenter.getPreviousImpactedGroupMappings();
            if (previousMappedGroups != null && previousMappedGroups.Count > AppConsts.NONE)
            {
                foreach (RuleImpactGroupMapping previousMappedGroup in previousMappedGroups)
                {
                    if (previousMappedGroup.RUGM_UserGroupId != null)
                    {
                        cmbUserGroup.FindItemByValue(String.Concat(UserGroupType.Defined.GetStringValue(), previousMappedGroup.RUGM_UserGroupId)).Checked = true;
                    }
                    else
                    {
                        Int32 definedUserGroupId = AppConsts.NONE;
                        if (previousMappedGroup.lkpRuleImpactGroup.Code == FixedUserGroups.All.GetStringValue())
                        {
                            definedUserGroupId = Convert.ToInt32(FixedUserGroups.All);
                        }
                        else if (previousMappedGroup.lkpRuleImpactGroup.Code == FixedUserGroups.Existing.GetStringValue())
                        {
                            definedUserGroupId = Convert.ToInt32(FixedUserGroups.Existing);
                        }
                        else if (previousMappedGroup.lkpRuleImpactGroup.Code == FixedUserGroups.New.GetStringValue())
                        {
                            definedUserGroupId = Convert.ToInt32(FixedUserGroups.New);
                        }
                        cmbUserGroup.FindItemByValue(String.Concat(UserGroupType.Fixed.GetStringValue(), definedUserGroupId)).Checked = true;
                    }
                }
            }
        }

        public void assignRuleToUserGroups()
        {
            String userGroupIdList = String.Empty;
            //if All or new is selected
            if (IsAllSelected || IsNewSelected)
            {
                if (IsVersionUpdate)
                {
                    //Set all version's iscurrent=false
                }
                //set current version's iscurrent=true

            }

            //if all or existing is selected
            if (IsAllSelected || IsExistingSelected)
            {
                if (IsVersionUpdate)
                {
                    //deactivate all instance of previous version in package subscription rule
                }
                //create new instance for all the applicants of package in package subscription rule
            }

            //if all and  existing is not selected 
            else
            {
                if (IsVersionUpdate)
                {
                    //deactivate all instance of previous version in package subscription rule for applicant of selected user groups
                }
                //create new instance for selected applicants of package in package subscription rule

                List<Int32> userGroupId = new List<Int32>();
                foreach (var item in cmbUserGroup.CheckedItems)
                {
                    UserGroupContract userGroup = UserGroups.FirstOrDefault(x => x.UserGroupDataId == item.Value && x.UserGroupId != null);
                    if (userGroup != null)
                    {
                        userGroupId.Add(userGroup.UserGroupId.Value);
                    }
                }
                if (userGroupId.Count > 0)
                {
                    userGroupIdList = String.Join(",", userGroupId);
                }
            }
            Presenter.CreateXmlForVersionSettings(PackageId, userGroupIdList);
            Presenter.DeactivatePreviousRulesAndCreateNewRule(CurrentViewContext.SettingXml);
            UpdateRuleImpactedGroupsMapping();
        }

        private void UpdateRuleImpactedGroupsMapping()
        {
            List<lkpRuleImpactGroup> impactedGroupType = Presenter.getImpactedUserGroupType();
            List<RuleImpactGroupMapping> ruleImpactedGroups = new List<RuleImpactGroupMapping>();
            foreach (var item in cmbUserGroup.CheckedItems)
            {
                Int32 definedUserGroupTypeId = impactedGroupType.FirstOrDefault(x => x.Type == UserGroupType.Defined.GetStringValue()).RuleImpactGroupId;
                UserGroupContract userGroup = CurrentViewContext.UserGroups.FirstOrDefault(x => x.UserGroupDataId == item.Value && x.UserGroupId != null);
                if (userGroup != null)
                {
                    RuleImpactGroupMapping ruleImpactedGroup = new RuleImpactGroupMapping();
                    ruleImpactedGroup.RUGM_RuleMappingId = CurrentViewContext.RuleMappingId;
                    ruleImpactedGroup.RUGM_RuleImpactGroupId = definedUserGroupTypeId;
                    ruleImpactedGroup.RUGM_UserGroupId = userGroup.UserGroupId;
                    ruleImpactedGroups.Add(ruleImpactedGroup);
                }
                UserGroupContract allGroup = UserGroups.FirstOrDefault(x => x.UserGroupDataId == item.Value && item.Value == String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.All)));
                if (allGroup != null)
                {
                    Int32 allGroupTypeId = impactedGroupType.FirstOrDefault(x => x.Code == FixedUserGroups.All.GetStringValue()).RuleImpactGroupId;
                    RuleImpactGroupMapping ruleImpactedGroup = new RuleImpactGroupMapping();
                    ruleImpactedGroup.RUGM_RuleMappingId = CurrentViewContext.RuleMappingId;
                    ruleImpactedGroup.RUGM_RuleImpactGroupId = allGroupTypeId;
                    ruleImpactedGroup.RUGM_UserGroupId = allGroup.UserGroupId;
                    ruleImpactedGroups.Add(ruleImpactedGroup);
                }
                UserGroupContract newGroup = UserGroups.FirstOrDefault(x => x.UserGroupDataId == item.Value && item.Value == String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.New)));
                if (newGroup != null)
                {
                    Int32 newGroupTypeId = impactedGroupType.FirstOrDefault(x => x.Code == FixedUserGroups.New.GetStringValue()).RuleImpactGroupId;
                    RuleImpactGroupMapping ruleImpactedGroup = new RuleImpactGroupMapping();
                    ruleImpactedGroup.RUGM_RuleMappingId = CurrentViewContext.RuleMappingId;
                    ruleImpactedGroup.RUGM_RuleImpactGroupId = newGroupTypeId;
                    ruleImpactedGroup.RUGM_UserGroupId = newGroup.UserGroupId;
                    ruleImpactedGroups.Add(ruleImpactedGroup);
                }
                UserGroupContract existingGroup = UserGroups.FirstOrDefault(x => x.UserGroupDataId == item.Value && item.Value == String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.Existing)));
                if (existingGroup != null)
                {
                    Int32 existingGroupTypeId = impactedGroupType.FirstOrDefault(x => x.Code == FixedUserGroups.Existing.GetStringValue()).RuleImpactGroupId;
                    RuleImpactGroupMapping ruleImpactedGroup = new RuleImpactGroupMapping();
                    ruleImpactedGroup.RUGM_RuleMappingId = CurrentViewContext.RuleMappingId;
                    ruleImpactedGroup.RUGM_RuleImpactGroupId = existingGroupTypeId;
                    ruleImpactedGroup.RUGM_UserGroupId = existingGroup.UserGroupId;
                    ruleImpactedGroups.Add(ruleImpactedGroup);
                }
            }
            Presenter.updateRuleImpactedGroupMappings(ruleImpactedGroups);
        }

        private void SetCreateNewVersionButton()
        {
            if (Presenter.IsEditRuleAllowed())
            {
                fsucCmdBarRule.SubmitButton.Enabled = false;
                fsucCmdBarRule.ClearButton.Visible = true;
                fsucCmdBarRule.ClearButtonText = "Create New Version";
                fsucCmdBarRule.ReloadButtonText();
            }
        }

        private void BindSharedInstances()
        {
            Presenter.GetListOfInstanceWichCanShareRuleOnEdit();
            if (cmbShareBetween.CheckedItems.Count > 0)
                cmbShareBetween.ClearCheckedItems();
            if (!RuleSynchronisationData.IfRuleIsAlreadyShared && (RuleSynchronisationData.PkgListCanShareRuleInstance == null || RuleSynchronisationData.PkgListCanShareRuleInstance.Count == AppConsts.NONE))
            {
                dvSharedInstance.Visible = false;
            }
            else
            {
                dvSharedInstance.Visible = true;
                dvUpdateOption.Visible = true;

                if (RuleSynchronisationData.IfRuleIsAlreadyShared)
                    dvUpdateOption.Visible = true;
                else
                    dvUpdateOption.Visible = false;
                if (RuleSynchronisationData.PkgListCanShareRuleInstance != null
                    && RuleSynchronisationData.PkgListCanShareRuleInstance.Count > AppConsts.NONE)
                {
                    dvAddTo.Visible = true;
                    cmbShareBetween.DataSource = RuleSynchronisationData.PkgListCanShareRuleInstance;
                    cmbShareBetween.DataValueField = "CompliancePackageId";
                    cmbShareBetween.DataTextField = "PackageName";
                    cmbShareBetween.DataBind();
                }
                else
                    dvAddTo.Visible = false;
            }
        }
        #endregion
        #endregion



    }
}

