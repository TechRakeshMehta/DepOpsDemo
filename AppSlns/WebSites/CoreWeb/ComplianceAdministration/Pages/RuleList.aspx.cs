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
    public partial class RuleList : System.Web.UI.Page, IRuleListView
    {
        #region Variable

        #region Private Variables
        private RuleListPresenter _presenter = new RuleListPresenter();
        Int32 tenantid = 0;
        private ComplianceDataStore _complinceDataStore;
        private List<lkpRuleObjectMappingType> _listRuleObjectMappingType;
        private List<lkpConstantType> _constantTypeList;
        private Boolean _isScheduleActionRecordInserted = false;
        #endregion

        #region Public variables

        #endregion
        #endregion

        #region Properties

        public RuleListPresenter Presenter
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

        public IRuleListView CurrentViewContext
        {
            get { return this; }
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

        //public List<RuleMapping> lstRuleMapping
        public List<RuleMappingContract> lstRuleMapping
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

        Int32 IRuleListView.CurrentLoggedInUserId
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
                ViewState["IsVersionUpdate"] = false;
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

        public List<CompliancePackage> PackageListForSharingRuleInstance
        {
            get
            {
                if (!(ViewState["PackageListForSharingRuleInstance"] is List<CompliancePackage>))
                {
                    ViewState["PackageListForSharingRuleInstance"] = new List<CompliancePackage>();
                }
                return (List<CompliancePackage>)ViewState["PackageListForSharingRuleInstance"];
            }
            set
            {
                ViewState["PackageListForSharingRuleInstance"] = value;
            }
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

        public Int32 ObjectId
        {
            get
            {
                if (ViewState["ObjectId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["ObjectId"]);
            }
            set
            {
                ViewState["ObjectId"] = value;
            }
        }
        public String ObjectType
        {
            get
            {
                if (ViewState["ObjectType"] == null)
                    return String.Empty;
                return Convert.ToString(ViewState["ObjectType"]);
            }
            set
            {
                ViewState["ObjectType"] = value;
            }
        }


        public List<RuleSetData> lstRuleSetAssociationData
        {
            get;
            set;
        }
        #endregion

        #region Private Properties
        //UAT-2147 Auto-fill in dropdowns on rule mapping on tracking side
        Int32 IRuleListView.CurrentCategoryID
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

        Int32 IRuleListView.CurrentItemID
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentItemID"]);
            }
            set
            {
                ViewState["CurrentItemID"] = value;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

                if (Request.QueryString["PackageId"] != null)
                {
                    PackageId = Convert.ToInt32(Request.QueryString["PackageId"]);
                    hdnfPackageId.Value = PackageId.ToString();
                }
                if (Request.QueryString["Id"] != null)
                    RuleSetId = Convert.ToInt32(Request.QueryString["Id"]);

                if (Request.QueryString["SelectedTenantId"] != null)
                {
                    SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    hdnfSelectedtenantId.Value = SelectedTenantId.ToString();
                }
                if (Request.QueryString["CurrentCategoryID"] != null)
                {
                    CurrentViewContext.CurrentCategoryID = Convert.ToInt32(Request.QueryString["CurrentCategoryID"]);
                    hdnfCategoryId.Value = CurrentViewContext.CurrentCategoryID.ToString();
                    if (Request.QueryString["CurrentCategoryID"] != AppConsts.ZERO)
                    {
                        ObjectId = Convert.ToInt32(Request.QueryString["CurrentCategoryID"]);
                        ObjectType = INTSOF.Utils.ObjectType.Compliance_Category.GetStringValue();
                    }
                }
                if (Request.QueryString["CurrentItemID"] != null && Request.QueryString["CurrentItemID"] != AppConsts.ZERO)
                {
                    CurrentViewContext.CurrentItemID = Convert.ToInt32(Request.QueryString["CurrentItemID"]);
                    hdnfitemId.Value = Request.QueryString["CurrentItemID"];
                    ObjectId = Convert.ToInt32(Request.QueryString["CurrentItemID"]);
                    ObjectType = INTSOF.Utils.ObjectType.Compliance_Item.GetStringValue();
                }
                if (Request.QueryString["CurrentAttributeID"] != null && Request.QueryString["CurrentAttributeID"] != AppConsts.ZERO)
                {
                    hdnfAttributeId.Value = Request.QueryString["CurrentAttributeID"];
                    ObjectId = Convert.ToInt32(Request.QueryString["CurrentAttributeID"]);
                    ObjectType = INTSOF.Utils.ObjectType.Compliance_ATR.GetStringValue();
                }
                Presenter.GetRuleTemplates();
                BindTemplates();
                IfAnySubscriptionExist = Presenter.IsAnySubscriptionExist(PackageId);
                if (IfAnySubscriptionExist)
                {
                    dvUsergroup.Visible = true;
                    bindUserGroup();
                    rfvUserGroup.Enabled = true;
                }
                else
                {
                    dvUsergroup.Visible = false;
                    rfvUserGroup.Enabled = false;
                }

                BindInstanceCanBeShared();

                chkActive.Checked = true;
                if (SelectedTenantId == DefaultTenantId)
                    ((GridButtonColumn)grdRules.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Are you sure you want to delete this record?";
                else
                    ((GridButtonColumn)grdRules.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Deleting this record will impact existing orders. Are you sure you want to delete?";

            }
            LoadExpressionObjects();

            lblSuccess.Visible = false;
            lblSuccess.Text = String.Empty;

            Presenter.OnViewLoaded();

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = true;
            ResetControls();
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

            litExpression.Text = RuleTemplateDetails.RLT_UIExpression;

            LoadExpressionObjects();
        }

        protected void cmdBarSaveRule_SaveClick(object sender, EventArgs e)
        {
            if (CurrentViewContext.RuleSetId == AppConsts.NONE)
            {
                Presenter.SaveNewRuleSet();
            }
            if (!String.IsNullOrEmpty(cmbMasterTemplates.SelectedValue))
            {
                RuleMapping mappingToBeSaved = GetRuleMapping();
                String errorMsg = String.Empty;
                List<Int32> packagelist = new List<Int32>();

                List<Int32> lstCategoryMappedinObjects = new List<Int32>();
                List<Tuple<Int32, Int32, Int32>> objectMappings = new List<Tuple<Int32, Int32, Int32>>();
                string catObjectType = INTSOF.Utils.ObjectType.Compliance_Category.GetStringValue();
                List<lkpObjectType> objectTypeList = Presenter.GetObjectTypeList();
                lkpObjectType objectTypeCat = objectTypeList.FirstOrDefault(x => x.OT_Code.Equals(catObjectType));

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
                }

                if (cmbShareBetween.CheckedItems.Count > 0)
                {
                    foreach (var checkedItem in cmbShareBetween.CheckedItems)
                    {
                        packagelist.Add(Convert.ToInt32(checkedItem.Value));
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
                    // Apply check here for Also Add To scenario 
                    if (!packagelist.IsNullOrEmpty())
                    {
                        Dictionary<String, String> unMappedObjects = Presenter.IsCategoriesAvailableinSelectedPackages(packagelist, lstCategoryMappedinObjects, objectMappings);

                        if (unMappedObjects.Count == 0)
                        {
                            ContinueToSaveRule(mappingToBeSaved, errorMsg, packagelist);
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
                        ContinueToSaveRule(mappingToBeSaved, errorMsg, packagelist);
                    }
                }
            }
        }

        private void ContinueToSaveRule(RuleMapping mappingToBeSaved, string errorMsg, List<int> packagelist)
        {
            RuleProcessingResult xmlOutputString = validateRule(mappingToBeSaved.RuleMappingDetails.ToList());
            if (xmlOutputString.Status == 1)
            {
                txtValidationResult.Text = String.Format("Error: {0}", xmlOutputString.ErrorMessage);
                btnValidate.Text = "Re-validate Rule";
            }
            else
            {
                CurrentViewContext.IsAllSelected = cmbUserGroup.CheckedItems.Any(x => x.Value == String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.All)));
                CurrentViewContext.IsNewSelected = cmbUserGroup.CheckedItems.Any(x => x.Value == String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.New)));
                CurrentViewContext.IsExistingSelected = cmbUserGroup.CheckedItems.Any(x => x.Value == String.Concat(UserGroupType.Fixed.GetStringValue(), Convert.ToInt32(FixedUserGroups.Existing)));
                // RuleMapping mappingToBeSaved = GetRuleMapping(xmlOutputString.UIExpressionLabel);
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
                Presenter.SaveRuleMapping(mappingToBeSaved);
                String settingsXml = String.Empty;
                if (IfAnySubscriptionExist)
                {
                    String userGroupIdList = GetUserGroupsListForAssignRule();
                    settingsXml = Presenter.CreateXmlForVersionSettings(PackageId, userGroupIdList);
                    Presenter.DeactivatePreviousRulesAndCreateNewRule(settingsXml);
                    UpdateRuleImpactedGroupsMapping();
                }

                if (cmbShareBetween.CheckedItems.Count > 0)
                {
                    Presenter.ComplianceRuleSynchronisation(packagelist, settingsXml, IfAnySubscriptionExist);
                }
                if (IsScheduleActionRecordInserted)
                {
                    Presenter.InsertSystemServiceTrigger();
                }

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

        protected void grdRules_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (CurrentViewContext.SelectedTenantId != CurrentViewContext.DefaultTenantId)
            {
                Presenter.GetRuleSetDataByObjectId(); //Changed
            }

            CurrentViewContext.RuleSetId = RuleSetId;
            Presenter.GetRuleMappings();
            grdRules.DataSource = lstRuleMapping;
            grdRules.Columns.FindByUniqueName("RLM_IsCurrent").Visible = !DefaultTenantId.Equals(SelectedTenantId);
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

            }
            catch (Exception ex)
            {

            }
        }

        //protected void grdRules_ItemCreated(object sender, GridItemEventArgs e)
        //{
        //    if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
        //    {
        //        if (e.Item is GridDataItem)
        //        {
        //            GridDataItem dataItem = (GridDataItem)e.Item;
        //            Int32 ruleMappingId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RLM_ID"]);

        //            if (Presenter.IsRuleAssociationExists(ruleMappingId))
        //            {
        //                if (CurrentViewContext.lstRuleSetAssociationData.IsNotNull())
        //                {
        //                    RuleSetData selectedRuleAssociationData = CurrentViewContext.lstRuleSetAssociationData.Where(x => x.RuleMappingId == ruleMappingId).FirstOrDefault();

        //                    if (CurrentViewContext.lstRuleSetAssociationData.IsNotNull() && (selectedRuleAssociationData.IsNull() || selectedRuleAssociationData.IsNotNull() && CurrentViewContext.lstRuleSetAssociationData.Count(x => x.RuleAssociationId == selectedRuleAssociationData.RuleAssociationId) == AppConsts.ONE))
        //                    {
        //                        (e.Item as GridDataItem)["DeleteRuleAssociation"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
        //                    }
        //                    else
        //                    {
        //                        (e.Item as GridDataItem)["DeleteColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                (e.Item as GridDataItem)["DeleteRuleAssociation"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
        //            }
        //        }
        //    }
        //}

        protected void grdRules_ItemCreated(object sender, GridItemEventArgs e)//grdRules_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    Int32 ruleMappingId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RLM_ID"]);
                    //Optimize
                    Boolean IsRuleAssociationExists = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsRuleAssociationExists"]);
                    //
                    //if (Presenter.IsRuleAssociationExists(ruleMappingId))
                    if (CurrentViewContext.SelectedTenantId != CurrentViewContext.DefaultTenantId && IsRuleAssociationExists)
                    {
                        if (CurrentViewContext.lstRuleSetAssociationData.IsNotNull())
                        {
                            RuleSetData selectedRuleAssociationData = CurrentViewContext.lstRuleSetAssociationData.Where(x => x.RuleMappingId == ruleMappingId).FirstOrDefault();

                            if (CurrentViewContext.lstRuleSetAssociationData.IsNotNull() && (selectedRuleAssociationData.IsNull() || selectedRuleAssociationData.IsNotNull() && CurrentViewContext.lstRuleSetAssociationData.Count(x => x.RuleAssociationId == selectedRuleAssociationData.RuleAssociationId) == AppConsts.ONE))
                            {
                                (e.Item as GridDataItem)["DeleteRuleAssociation"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                            }
                            else
                            {
                                (e.Item as GridDataItem)["DeleteColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                            }
                        }
                    }
                    else
                    {
                        (e.Item as GridDataItem)["DeleteRuleAssociation"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }
                }
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
            mapping.RLM_RuleSetID = RuleSetId;
            mapping.RLM_Name = txtRuleName.Text;
            mapping.RLM_ActionBlock = txtActionMapping.Text;
            //mapping.RLM_UIExpression = uiExpression;
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
                ExpressionObject expressionObject = (ExpressionObject)pnlExpressionObjects.FindControl(String.Format("EO_{0}", counter));
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

                            RuleMappingObjectTree ruleMappingObjectTreeForPackage = GetRuleMappingObjectTree(PackageId, RuleSetTreeType.Packages, ruleSetTree); ;
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

                            lkpRuleObjectMappingType ruleObjectMappingType = listRuleObjectMappingType.FirstOrDefault(x => x.RMT_Code == expressionObject.SelectedMappingTypeCode); ;
                            detail.RLMD_RuleObjectMappingTypeID = ruleObjectMappingType.RMT_ID;

                            RuleMappingObjectTree ruleMappingObjectTreeForPackage = GetRuleMappingObjectTree(PackageId, RuleSetTreeType.Packages, ruleSetTree); ;
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
                                detail.RLMD_ObjectID = Convert.ToInt32(expressionObject.SelectedItemSubmissionDateID);

                                lkpObjectType objectType = objectTypeList.FirstOrDefault(x => x.OT_Code == INTSOF.Utils.ObjectType.Compliance_Item.GetStringValue());
                                detail.RLMD_ObjectTypeID = objectType.OT_ID;

                                RuleMappingObjectTree ruleMappingObjectTreeForPackage = GetRuleMappingObjectTree(PackageId, RuleSetTreeType.Packages, ruleSetTree);
                                if (ruleMappingObjectTreeForPackage != null)
                                    detail.RuleMappingObjectTrees.Add(ruleMappingObjectTreeForPackage);

                                RuleMappingObjectTree ruleMappingObjectTreeForCategory = GetRuleMappingObjectTree(Convert.ToInt32(expressionObject.CurrentCategoryID), RuleSetTreeType.Categories, ruleSetTree);
                                if (ruleMappingObjectTreeForCategory != null)
                                    detail.RuleMappingObjectTrees.Add(ruleMappingObjectTreeForCategory);
                            }
                            detail.RLMD_ConstantValue = expressionObject.ConstantValue;

                            detail.RLMD_ConstantType = constantType.ID;
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
                    ExpressionObject ctrl = (ExpressionObject)LoadControl("~/ComplianceAdministration/UserControl/ExpressionObject.ascx");
                    ctrl.RowId = rowId;
                    ctrl.ID = String.Format("EO_{0}", rowId);
                    ctrl.PackageId = PackageId;
                    ctrl.SelectedTenantId = SelectedTenantId;
                    ctrl.complinceDataStore = ComplinceDataStore;
                    ctrl.lstRuleObjectMappingType = listRuleObjectMappingType;
                    //UAT-2147 Auto fill dropdown On selecting ddlCompliance Dropdown with current category as selected.
                    ctrl.CurrentCategoryID = CurrentViewContext.CurrentCategoryID;
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
                    // String constantTypeCode = Presenter.getConstantTypeCodeById(expressionObject.constantTypeId).Code;

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
            BindInstanceCanBeShared();
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

        private void bindUserGroup()
        {

            List<UserGroup> definedUserGroup = Presenter.getAllUsergroups();
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
            cmbUserGroup.DataSource = UserGroups;
            cmbUserGroup.DataTextField = "UserGroupName";
            cmbUserGroup.DataValueField = "UserGroupDataId";
            cmbUserGroup.DataBind();
            // cmbUserGroup.Items.Insert(0, new RadComboBoxItem("--SELECT--", String.Empty));
        }

        public String GetUserGroupsListForAssignRule()
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
            return userGroupIdList;
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
                    ruleImpactedGroup.RUGM_IsDeleted = false;
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
                    ruleImpactedGroup.RUGM_IsDeleted = false;
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
                    ruleImpactedGroup.RUGM_IsDeleted = false;
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
                    ruleImpactedGroup.RUGM_IsDeleted = false;
                    ruleImpactedGroups.Add(ruleImpactedGroup);
                }
            }
            Presenter.updateRuleImpactedGroupMappings(ruleImpactedGroups);
        }

        private void BindInstanceCanBeShared()
        {
            Presenter.GetListOfInstanceWichCanShareRule();
            if (CurrentViewContext.PackageListForSharingRuleInstance != null && CurrentViewContext.PackageListForSharingRuleInstance.Count > 0)
            {
                dvSharedInstance.Visible = true;
            }
            else
            {
                dvSharedInstance.Visible = false;
            }
            cmbShareBetween.DataSource = PackageListForSharingRuleInstance;
            cmbShareBetween.DataValueField = "CompliancePackageId";
            cmbShareBetween.DataTextField = "PackageName";
            cmbShareBetween.DataBind();
        }



        #endregion

        #region Protected Methods
        protected void btnRuleDeletionDoPostBack_Click(object sender, EventArgs e)
        {
            lblSuccess.Visible = true;
            lblSuccess.ShowMessage("Rule deleted successfully.", MessageType.SuccessMessage);
            grdRules.Rebind();
        }
        #endregion
        #endregion


    }
}

