using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using CoreWeb.Shell;
using Telerik.Web.UI;
using INTSOF.Utils;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using CoreWeb.IntsofSecurityModel;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ExpressionObject : BaseUserControl, IExpressionObjectView
    {
        private ExpressionObjectPresenter _presenter = new ExpressionObjectPresenter();
        private String _constantValue;
        private Int32? _constantTypeId;
        private ComplianceDataStore _complinceDataStore;
        //private variables to fill preloaded ExpressionObject
        public String objectMappingTypeCode { get; set; }
        public String objectTypeCode { get; set; }
        public Int32? categoryId { get; set; }
        public Int32? itemId { get; set; }
        public Int32? attributeId { get; set; }
        //UAT-2147 Auto-fill in dropdowns on rule mapping on tracking side
        public Int32? CurrentCategoryID { get; set; }
        #region Properties

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

            }
            Presenter.OnViewLoaded();
        }

        public IExpressionObjectView CurrentViewContext
        {
            get { return this; }
        }


        public ExpressionObjectPresenter Presenter
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

        Int32 rowId = 0;
        public Int32 RowId
        {
            get { return rowId; }
            set
            {
                rowId = value;
                ObjectName = ObjectName;
            }
        }

        public String ObjectName
        {
            get
            {
                return String.Format("[Object{0}]", this.RowId);
            }
            set
            {
                objName.Text = value;
            }
        }


        Int32 tenantid = 0;
        Int32 IExpressionObjectView.TenantId
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

        Int32 IExpressionObjectView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 PackageId
        {
            get
            {
                if (ViewState["PackageId"] != null)
                    return Convert.ToInt32(ViewState["PackageId"]);
                return 0;
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        public List<CompliancePackageCategory> lstCompliancePackageCategories
        {
            get;
            set;
        }

        public List<ComplianceCategoryItem> lstComplianceCategoryItems
        {
            get;
            set;
        }

        public List<ComplianceItemAttribute> lstComplianceItemAttributes
        {
            get;
            set;
        }

        public String SelectedMappingTypeCode
        {
            get
            {
                return ddlCompliance.SelectedValue;
            }
            set
            {
                ddlCompliance.SelectedValue = value;

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
                ddlObjectType.SelectedValue = value;

            }
        }

        public String ConstantValue
        {
            //get
            //{
            //    return txtConstant.Text;
            //}
            //set
            //{
            //    txtConstant.Text = value;
            //}
            get
            {
                if (selectedConstantTypeCode == ConstantType.Empty.GetStringValue())
                {
                    _constantValue = AppConsts.EMPTY;
                }
                else if (SelectedConstatGroupValue == "Personal Info")
                {
                    _constantValue = GenerateHashString(selectedConstantTypeCode);
                }
                else if (CurrentChecked)
                {
                    if (selectedConstantTypeCode == ConstantType.Year.GetStringValue())
                    {
                        _constantValue = AppConsts.CURRENT_YEAR;
                    }
                    if (selectedConstantTypeCode == ConstantType.Month.GetStringValue())
                    {
                        _constantValue = AppConsts.CURRENT_MONTH;
                    }
                    if (selectedConstantTypeCode == ConstantType.Day.GetStringValue())
                    {
                        _constantValue = AppConsts.CURRENT_DAY;
                    }
                }
                else if (selectedConstantTypeCode == ConstantType.ItemComplianceStatus.GetStringValue())
                {
                    _constantValue = SelectedItemComplianceStatusId;
                }
                else if (selectedConstantTypeCode == ConstantType.ItemSubmissionDate.GetStringValue())
                {
                    _constantValue = AppConsts.SUBMISSION_DATE;
                }
                else
                {
                    _constantValue = txtConstant.Text;
                }
                return _constantValue;
            }
            set
            {
                _constantValue = value;
                if (_constantValue == AppConsts.CURRENT_DAY || _constantValue == AppConsts.CURRENT_MONTH
                    || _constantValue == AppConsts.CURRENT_YEAR || _constantValue == AppConsts.EMPTY)
                {
                    CurrentChecked = true;
                }
                else if (selectedConstantTypeCode == ConstantType.ItemComplianceStatus.GetStringValue())
                {
                    SelectedItemComplianceStatusId = _constantValue;
                }
                else
                {
                    txtConstant.Text = _constantValue;
                }
            }
        }

        public Int32 SelectedCategoryId
        {
            get
            {
                return Convert.ToInt32(ddlCategory.SelectedValue);
            }
            set
            {
                ddlCategory.SelectedValue = value.ToString();

            }
        }

        public Int32 SelectedItemId
        {
            get
            {
                return Convert.ToInt32(ddlItem.SelectedValue);
            }
            set
            {
                ddlItem.SelectedValue = value.ToString();
            }
        }

        public String SelectedItemName
        {
            get
            {
                return ddlItem.SelectedText;
            }
            set
            {
                ddlItem.SelectedText = value.ToString();
            }
        }
        public Int32 SelectedAttributeId
        {
            get
            {
                return Convert.ToInt32(ddlAttribute.SelectedValue);
            }
            set
            {
                ddlAttribute.SelectedValue = value.ToString();
            }
        }

        public String SelectedAttributeName
        {
            get
            {
                return ddlAttribute.SelectedText;
            }
            set
            {
                ddlAttribute.SelectedText = value.ToString();
            }
        }
        List<lkpRuleObjectMappingType> IExpressionObjectView.lstRuleObjectMappingType
        {
            get;
            set;
        }

        public List<lkpObjectType> lstObjectType
        {
            get;
            set;
        }

        List<lkpRuleObjectMappingType> ruleObjectMappingType;
        public List<lkpRuleObjectMappingType> lstRuleObjectMappingType
        {
            get { return ruleObjectMappingType; }
            set
            {
                ruleObjectMappingType = value;

                AddDefaultItem(ddlObjectType, AppConsts.COMBOBOX_ITEM_SELECT);
                ddlCompliance.DataSource = ruleObjectMappingType;
                ddlCompliance.DataTextField = "RMT_Name";
                ddlCompliance.DataValueField = "RMT_Code";
                ddlCompliance.DataBind();
                AddDefaultItem(ddlCompliance, AppConsts.COMBOBOX_ITEM_SELECT);
                if (!String.IsNullOrEmpty(this.SelectedMappingTypeCode))
                {
                    //this.SelectedMappingTypeCode = objectMappingTypeCode;
                    BindControlForComplianceType();
                }
            }
        }

        public List<lkpConstantType> lstConstantType
        {
            get;
            set;
        }

        public List<lkpConstantType> lstConstantGroup
        {
            get;
            set;
        }

        public String selectedConstantTypeCode
        {
            get
            {
                return ddlConstantType.SelectedValue;
            }
            set
            {
                ddlConstantType.SelectedValue = value;
            }
        }

        public String SelectedConstatGroupValue
        {
            get
            {
                return ddlConstantGroup.SelectedValue;
            }
            set
            {
                ddlConstantGroup.SelectedValue = value;
            }
        }

        public Boolean CurrentChecked
        {
            get
            {
                return chkCurrent.Checked;
            }
            set
            {
                chkCurrent.Checked = value;
            }

        }

        public Int32? constantTypeId
        {
            get { return _constantTypeId; }
            set
            {
                _constantTypeId = value;
                //if (_constantTypeId != null)
                //{
                //    lkpConstantType constantType = Presenter.getConstantTypeById(_constantTypeId);
                //    SelectedConstatGroupValue = constantType.Group;
                //    selectedConstantTypeCode = constantType.Code;
                //}
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

        public ComplianceDataStore complinceDataStore
        {
            get
            {
                return _complinceDataStore;
            }
            set
            {
                _complinceDataStore = value;
            }
        }

        public List<lkpItemComplianceStatu> lstItemComplianceStatus
        {
            get;
            set;
        }

        public String SelectedItemComplianceStatusId
        {
            get
            {
                return ddlItemStatus.SelectedValue;
            }
            set
            {
                ddlItemStatus.SelectedValue = value.ToString();
            }
        }

        public String SelectedItemSubmissionDateID
        {
            get
            {
                return ddlItemSubmission.SelectedValue;
            }
            set
            {
                ddlItemSubmission.SelectedValue = value.ToString();
            }
        }

        protected void ddlCompliance_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentViewContext.SelectedMappingTypeCode = ddlCompliance.SelectedValue;
            ddlConstantGroup.SelectedValue = "0";
            ddlConstantType.SelectedValue = "0";
            ConstantValue = String.Empty;
            BindControlForComplianceType();
        }

        protected void ddlObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCategory.SelectedValue = "0";
            ddlItem.SelectedValue = "0";
            ddlAttribute.SelectedValue = "0";
            BindForObjectType();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlItem.SelectedValue = "0";
            ddlAttribute.SelectedValue = "0";
            BindItems();
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAttribute.SelectedValue = "0";
            BindAttributes();
        }

        private void BindControlForComplianceType()
        {
            ddlConstantGroup.Visible = false;
            ddlConstantType.Visible = false;
            ddlItemStatus.Visible = false;
            ddlItemSubmission.Visible = false;
            //ddlConstantGroup.SelectedValue = "0";
            //ddlConstantType.SelectedValue = "0";
            ShowHideCostantTextBox(false);
            ShowHideCheckBox(false);

            if (CurrentViewContext.SelectedMappingTypeCode.ToLower() == ObjectMappingType.Compliance_Value.GetStringValue().ToLower())
            {
                BindObjectType();
                ManageForm(true, false);
                if (objectTypeCode.IsNullOrEmpty())
                    objectTypeCode = ObjectType.Compliance_Item.GetStringValue();
            }
            else if (CurrentViewContext.SelectedMappingTypeCode.ToLower() == ObjectMappingType.Data_Value.GetStringValue().ToLower())
            {
                BindObjectType();
                ManageForm(true, false);
                if (objectTypeCode.IsNullOrEmpty())
                    objectTypeCode = ObjectType.Compliance_ATR.GetStringValue();
            }
            else if (CurrentViewContext.SelectedMappingTypeCode.ToLower() == ObjectMappingType.Defined_Value.GetStringValue().ToLower())
            {
                BindDataForConstant();
                ManageForm(false, true);
            }
            else
            {
                ManageForm(true, false);
                ddlObjectType.Items.Clear();
                AddDefaultItem(ddlObjectType, AppConsts.COMBOBOX_ITEM_SELECT);
            }

            ddlAttribute.Visible = false;
            ddlItem.Visible = false;
            ddlCategory.Visible = false;
            ddlCategory.SelectedValue = "0";
            ddlAttribute.SelectedValue = "0";
            ddlItem.SelectedValue = "0";
            ddlObjectType.SelectedValue = "0";

            if (!String.IsNullOrEmpty(this.objectTypeCode))
            {
                this.SelectedObjectTypeCode = objectTypeCode;
                BindForObjectType();
            }
        }

        protected void ddlConstantGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlConstantType.Items.Clear();
            ddlConstantType.SelectedValue = "0";
            ConstantValue = String.Empty;
            CurrentChecked = false;
            ShowHideCostantTextBox(false);
            ShowHideCheckBox(false);
            BindConstantType();
            ddlItemStatus.Items.Clear();
            ddlItemStatus.SelectedValue = "0";
            ddlItemStatus.Visible = false;
            ddlItemSubmission.Items.Clear();
            ddlItemSubmission.SelectedValue = "0";
            ddlItemSubmission.Visible = false;
        }

        protected void ddlConstantType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConstantValue = String.Empty;
            CurrentChecked = false;
            ShowHideTextBoxOnBasisOfConstantType();
        }

        protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
        {
            ConstantValue = String.Empty;
            ShowHideTextBoxOnCheckBoxChange();
        }

        private void BindForObjectType()
        {
            if (ddlObjectType.SelectedValue.ToLower() == ObjectType.Compliance_Package.GetStringValue().ToLower())
            {
            }
            else if (ddlObjectType.SelectedValue.ToLower() == ObjectType.Compliance_Category.GetStringValue().ToLower())
            {
            }
            else if (ddlObjectType.SelectedValue.ToLower() == ObjectType.Compliance_Item.GetStringValue().ToLower())
            {
                ddlItem.Visible = true;
                ddlItem.AutoPostBack = false;
                ddlAttribute.Visible = false;
            }
            else if (ddlObjectType.SelectedValue.ToLower() == ObjectType.Compliance_ATR.GetStringValue().ToLower())
            {
                ddlItem.Visible = true;
                ddlItem.AutoPostBack = true;
                ddlAttribute.Visible = true;
                ddlAttribute.Items.Clear();
                AddDefaultItem(ddlAttribute, "-- SELECT ATTRIBUTE --");
            }
            ddlItem.Items.Clear();
            AddDefaultItem(ddlItem, "-- SELECT ITEM --");
            ddlCategory.Visible = true;
            BindCategories();

        }

        private void BindCategories()
        {
            //Entity.ClientEntity.CompliancePackageCategory cpc = new Entity.ClientEntity.CompliancePackageCategory();
            //cpc.ComplianceCategory.CategoryName
            //CurrentViewContext.lstCompliancePackageCategories = Presenter.GetCompliancePackageCategory(PackageId);
            CurrentViewContext.lstCompliancePackageCategories = complinceDataStore.getComplianceCategoriesByPackageId(PackageId, SelectedTenantId);
            ddlCategory.DataSource = CurrentViewContext.lstCompliancePackageCategories.Select(categories => categories.ComplianceCategory);
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "ComplianceCategoryID";
            ddlCategory.DataBind();
            AddDefaultItem(ddlCategory, "-- SELECT CATEGORY --");
            if ((this.categoryId != null))
            {
                this.SelectedCategoryId = categoryId.Value;
                BindItems();
            }
            else if (CurrentCategoryID.Value != null && ddlObjectType.SelectedValue != "0")
            {
                this.SelectedCategoryId = CurrentCategoryID.Value;
                BindItems();
            }
        }

        private void BindItems()
        {
            //lstComplianceCategoryItems = Presenter.GetComplianceCategoryItem(Convert.ToInt32(ddlCategory.SelectedValue));
            lstComplianceCategoryItems = complinceDataStore.getComplianceItemsByCategoryId(Convert.ToInt32(ddlCategory.SelectedValue), SelectedTenantId);
            ddlItem.DataSource = lstComplianceCategoryItems.Select(items => items.ComplianceItem); ;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ComplianceItemID";
            ddlItem.DataBind();
            AddDefaultItem(ddlItem, "-- SELECT ITEM --");

            if (this.itemId > 0)
            {
                this.SelectedItemId = itemId.Value;
                BindAttributes();
            }
        }

        private void BindAttributes()
        {
            //lstComplianceItemAttributes = Presenter.GetComplianceItemAttribute(Convert.ToInt32(ddlItem.SelectedValue));
            lstComplianceItemAttributes = complinceDataStore.getComplianceAttributesByItemsId(Convert.ToInt32(ddlItem.SelectedValue), SelectedTenantId);
            ddlAttribute.DataSource = lstComplianceItemAttributes.Select(attr => attr.ComplianceAttribute); ;
            ddlAttribute.DataTextField = "Name";
            ddlAttribute.DataValueField = "ComplianceAttributeID";
            ddlAttribute.DataBind();
            AddDefaultItem(ddlAttribute, "-- SELECT ATTRIBUTE --");
            if ((this.attributeId > 0))
            {
                this.SelectedAttributeId = attributeId.Value;

            }
        }

        public void BindRuleObjectMappingType()
        {
            AddDefaultItem(ddlObjectType, AppConsts.COMBOBOX_ITEM_SELECT);

            Presenter.GetRuleObjectMappingType();
            ddlCompliance.DataSource = CurrentViewContext.lstRuleObjectMappingType;
            ddlCompliance.DataTextField = "RMT_Name";
            ddlCompliance.DataValueField = "RMT_Code";
            ddlCompliance.DataBind();
            AddDefaultItem(ddlCompliance, AppConsts.COMBOBOX_ITEM_SELECT);
        }

        private void BindObjectType()
        {
            //_presenter.GetObjectType();
            CurrentViewContext.lstObjectType = complinceDataStore.getObjectType(SelectedMappingTypeCode, SelectedTenantId);
            ddlObjectType.DataSource = CurrentViewContext.lstObjectType;
            ddlObjectType.DataTextField = "OT_Description";
            ddlObjectType.DataValueField = "OT_Code";
            ddlObjectType.DataBind();
            AddDefaultItem(ddlObjectType, AppConsts.COMBOBOX_ITEM_SELECT);
        }

        private void AddDefaultItem(INTERSOFT.WEB.UI.WebControls.WclDropDownList ddTarget, String textToInsert)
        {
            ddTarget.Items.Insert(0, new DropDownListItem { Text = textToInsert, Value = AppConsts.ZERO });
        }

        private void ManageForm(Boolean ddVisibility, Boolean constantGrpVisibility)
        {
            ddlObjectType.Visible = ddVisibility;
            ddlConstantGroup.Visible = constantGrpVisibility;
            ddlConstantType.Visible = constantGrpVisibility;
        }

        private void BindConstantGroup()
        {
            //_presenter.getConstantGroup();
            CurrentViewContext.lstConstantGroup = complinceDataStore.getConstantGroup(SelectedTenantId, false);
            ddlConstantGroup.DataSource = CurrentViewContext.lstConstantGroup.GroupBy(x => x.Group).Distinct().ToList();
            ddlConstantGroup.DataTextField = "Key";
            ddlConstantGroup.DataValueField = "Key";
            ddlConstantGroup.DataBind();
            AddDefaultItem(ddlConstantGroup, "-- SELECT GROUP --");
            //if (selectedConstantTypeCode != String.Empty)
            //{
            //    BindConstantType();
            //    ShowHideTextBoxOnBasisOfConstantType();
            //}
            if (_constantTypeId != null)
            {
                lkpConstantType constantType = CurrentViewContext.lstConstantGroup.FirstOrDefault(x => x.ID == _constantTypeId);
                SelectedConstatGroupValue = constantType.Group;
                selectedConstantTypeCode = constantType.Code;
                if (selectedConstantTypeCode != String.Empty)
                {
                    BindConstantType();
                    if (selectedConstantTypeCode == ConstantType.ItemComplianceStatus.GetStringValue())
                    {
                        SelectedItemComplianceStatusId = _constantValue;
                    } 
                    ShowHideTextBoxOnBasisOfConstantType();
                }
            }
        }

        private void BindConstantType()
        {
            //_presenter.getConstantType(SelectedConstatGroupValue);
            CurrentViewContext.lstConstantType = complinceDataStore.getConstantType(SelectedConstatGroupValue, SelectedTenantId, false);
            ddlConstantType.DataSource = CurrentViewContext.lstConstantType;
            ddlConstantType.DataTextField = "Name";
            ddlConstantType.DataValueField = "Code";
            ddlConstantType.DataBind();
            AddDefaultItem(ddlConstantType, "-- SELECT CONSTANT TYPE --");
        }

        private void BindItemComplianceStatus()
        {
            lstItemComplianceStatus = complinceDataStore.getItemComplianceStatusList(SelectedTenantId);
            ddlItemStatus.DataSource = lstItemComplianceStatus;
            ddlItemStatus.DataTextField = "Name";
            ddlItemStatus.DataValueField = "ItemComplianceStatusID";
            ddlItemStatus.DataBind();
            AddDefaultItem(ddlItemStatus, "-- SELECT STATUS --");
        }

        private void BindDataForConstant()
        {
            ddlConstantType.Visible = true;
            ddlConstantType.AutoPostBack = true;
            AddDefaultItem(ddlConstantType, "-- SELECT CONSTANT TYPE --");
            ddlConstantGroup.Visible = true;
            ddlConstantGroup.AutoPostBack = true;
            BindConstantGroup();
        }

        private void BindSubmissionDateItem()
        {
            lstComplianceCategoryItems = complinceDataStore.getComplianceItemsByCategoryId(Convert.ToInt32(this.CurrentCategoryID), SelectedTenantId);
            ddlItemSubmission.DataSource = lstComplianceCategoryItems.Select(items => items.ComplianceItem);
            ddlItemSubmission.DataTextField = "Name";
            ddlItemSubmission.DataValueField = "ComplianceItemID";
            ddlItemSubmission.DataBind();
            AddDefaultItem(ddlItemSubmission, "-- SELECT ITEM --"); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowHideTextBoxOnBasisOfConstantType()
        {
            if (ddlConstantType.SelectedValue == ConstantType.Empty.GetStringValue()
                || ddlConstantType.SelectedValue == ConstantType.DOB.GetStringValue())
            {
                ShowHideCostantTextBox(false);
                ShowHideCheckBox(false);
            }
            else if (ddlConstantType.SelectedValue == ConstantType.Day.GetStringValue()
                 || ddlConstantType.SelectedValue == ConstantType.Month.GetStringValue()
                 || ddlConstantType.SelectedValue == ConstantType.Year.GetStringValue())
            {
                chkCurrent.AutoPostBack = true;
                ShowHideCostantTextBox(true);
                ShowHideCheckBox(true);
                if (_constantValue == AppConsts.CURRENT_DAY || _constantValue == AppConsts.CURRENT_MONTH
                   || _constantValue == AppConsts.CURRENT_YEAR || _constantValue == AppConsts.EMPTY)
                {
                    ShowHideCostantTextBox(false);
                }
            }
            else if (ddlConstantType.SelectedValue == ConstantType.Date.GetStringValue()
                 || ddlConstantType.SelectedValue == ConstantType.Numeic.GetStringValue()
                 || ddlConstantType.SelectedValue == ConstantType.Text.GetStringValue()
                || ddlConstantType.SelectedValue == ConstantType.Bool.GetStringValue())
            {
                ShowHideCostantTextBox(true);
                ShowHideCheckBox(false);
            }
            else if (ddlConstantType.SelectedValue == ConstantType.ItemSubmissionDate.GetStringValue())
            { 
                    ddlItemSubmission.Visible = true;
                    BindSubmissionDateItem(); 
            }
            else
            {
                ShowHideCostantTextBox(false);
                ShowHideCheckBox(false);
                if (ddlConstantType.SelectedValue == ConstantType.ItemComplianceStatus.GetStringValue())
                {
                    ddlItemStatus.Visible = true;
                    BindItemComplianceStatus();
                }
                ddlItemSubmission.Visible = false;
                ddlItemSubmission.SelectedValue = AppConsts.ZERO;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowHideTextBoxOnCheckBoxChange()
        {
            if (CurrentChecked)
            {
                ShowHideCostantTextBox(false);
            }
            else
            {
                ShowHideCostantTextBox(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtVisiblity"></param>
        /// <param name="chkCurrentVisiblity"></param>
        protected void ShowHideCostantTextBox(Boolean txtVisiblity)
        {
            txtConstant.Visible = txtVisiblity;

            if (txtConstant.Visible)
            {
                if (this.ConstantValue != String.Empty)
                    txtConstant.Text = this.ConstantValue;
                else
                    txtConstant.Text = String.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtVisiblity"></param>
        /// <param name="chkCurrentVisiblity"></param>
        protected void ShowHideCheckBox(Boolean chkCurrentVisiblity)
        {
            chkCurrent.Visible = chkCurrentVisiblity;
            if (chkCurrent.Visible)
            {
                chkCurrent.Checked = CurrentChecked;
            }
        }

        public Boolean IsExpressionObjectIsValid()
        {
            if (ddlCompliance.SelectedValue == String.Empty)
            {
                return false;
            }
            if (ddlObjectType.Visible == true)
            {
                if (ddlObjectType.SelectedValue == AppConsts.ZERO)
                    return false;
                if (ddlCategory.SelectedValue == AppConsts.ZERO)
                    return false;
                if (ddlItem.SelectedValue == AppConsts.ZERO)
                    return false;
            }
            if (ddlAttribute.Visible == true)
            {
                if (ddlAttribute.SelectedValue == AppConsts.ZERO)
                    return false;
            }
            if (ddlConstantGroup.Visible == true)
            {
                if (ddlConstantGroup.SelectedValue == AppConsts.ZERO || ddlConstantType.SelectedValue == AppConsts.ZERO)
                    return false;
            }
            if (txtConstant.Visible)
            {
                if (txtConstant.Text == String.Empty)
                    return false;
            }

            return true;
        }

        public String GenerateHashString(String constantCode)
        {
            return String.Format("{0}{1}{2}", AppConsts.DOLLAR, constantCode, AppConsts.DOLLAR);
        }
    }
}

