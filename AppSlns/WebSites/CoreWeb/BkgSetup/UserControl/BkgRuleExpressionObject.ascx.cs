using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.SharedObjects;

namespace CoreWeb.BkgSetup.Views
{
    public partial class BkgRuleExpressionObject : BaseUserControl, IBkgRuleExpressionObjectView
    {
        #region Private Variables

        private BkgRuleExpressionObjectPresenter _presenter = new BkgRuleExpressionObjectPresenter();
        Int32 _rowId = 0;
        Int32 _tenantid = 0;
        List<lkpBkgRuleObjectMappingType> _ruleObjectMappingType;
        private BkgDataStore _bkgDataStore;
        private String _constantValue;
        private Int32 _attributeGrpMappingId;
        private Int32 _selectedServiceId;
        private Int32? _constantTypeId;
        private List<GetAttributeListByPackageId> _attributeList;
        #endregion

        #region Properties

        public BkgRuleExpressionObjectPresenter Presenter
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

        public IBkgRuleExpressionObjectView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 RowId
        {
            get { return _rowId; }
            set
            {
                _rowId = value;
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

        Int32 IBkgRuleExpressionObjectView.TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
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

        Int32 IBkgRuleExpressionObjectView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 PackageId
        {
            get;
            set;
        }

        public String SelectedRuleObjectTypeCode
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

        public List<PackageService> lstBkgPackageSvc
        {
            get;
            set;
        }

        public Int32 SelectedServiceId
        {
            get
            {
                return Convert.ToInt32(ddlServices.SelectedValue);
            }
            set
            {
                ddlServices.SelectedValue = value.ToString();
                _selectedServiceId = value;
            }
        }

        public Int32 SelectedAttributeGrpId
        {
            get
            {
                return Convert.ToInt32(ddlAttributegrp.SelectedValue);
            }
            set
            {
                ddlAttributegrp.SelectedValue = value.ToString();
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

        public Int32 AttributeGrpMappingId
        {
            get
            {
                GetAttributeListByPackageId selectedAttribute = attributeList.FirstOrDefault(x => x.BkgAttributeGroupId == SelectedAttributeGrpId
                                                                    && x.BkgAttributeId == SelectedAttributeId);
                if (selectedAttribute != null)
                {
                    _attributeGrpMappingId = selectedAttribute.BkgAttributeGroupMappingId;
                    return _attributeGrpMappingId;
                }
                return AppConsts.NONE;
            }
            set
            {
                _attributeGrpMappingId = value;
            }
        }

        public BkgDataStore bkgDataStore
        {
            get
            {
                return _bkgDataStore;
            }
            set
            {
                _bkgDataStore = value;
            }
        }

        public List<lkpBkgConstantType> lstConstantType
        {
            get;
            set;
        }

        public List<lkpBkgConstantType> lstConstantGroup
        {
            get;
            set;
        }

        public List<lkpBkgRuleObjectMappingType> lstRuleObjectMappingType
        {
            get { return _ruleObjectMappingType; }
            set
            {
                _ruleObjectMappingType = value;
                ddlObjectType.DataSource = _ruleObjectMappingType;
                ddlObjectType.DataTextField = "BRMT_Name";
                ddlObjectType.DataValueField = "BRMT_Code";
                ddlObjectType.DataBind();
                AddDefaultItem(ddlObjectType, AppConsts.COMBOBOX_ITEM_SELECT);
                if (!(this.SelectedRuleObjectTypeCode == String.Empty))
                {
                    BindControlsOnTheBasisOfObjectType();
                }
            }
        }

        public List<GetAttributeListByPackageId> attributeList
        {
            get
            {
                if (_attributeList == null)
                {
                    _attributeList = bkgDataStore.getAttributeList(PackageId, SelectedTenantId);
                }
                return _attributeList;
            }
            set
            {
                _attributeList = value;
            }

        }

        public String SelectedConstantTypeCode
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

        public String ConstantValue
        {
            get
            {
                if (SelectedConstantTypeCode == BkgConstantType.EMPTY.GetStringValue())
                {
                    _constantValue = AppConsts.EMPTY;
                }
                else if (ddlConstantType.SelectedValue == BkgConstantType.CITY.GetStringValue()
                 || ddlConstantType.SelectedValue == BkgConstantType.COUNTY.GetStringValue()
                 || ddlConstantType.SelectedValue == BkgConstantType.STATE.GetStringValue()
                || ddlConstantType.SelectedValue == BkgConstantType.COUNTRY.GetStringValue())
                {
                    _constantValue = SelectedConstantValue;
                }
                else if (CurrentChecked)
                {
                    if (SelectedConstantTypeCode == ConstantType.Year.GetStringValue())
                    {
                        _constantValue = AppConsts.CURRENT_YEAR;
                    }
                    if (SelectedConstantTypeCode == ConstantType.Month.GetStringValue())
                    {
                        _constantValue = AppConsts.CURRENT_MONTH;
                    }
                    if (SelectedConstantTypeCode == ConstantType.Day.GetStringValue())
                    {
                        _constantValue = AppConsts.CURRENT_DAY;
                    }
                }
                else
                {
                    _constantValue = SelectedConstantText;
                }
                return _constantValue;
            }
            set
            {
                _constantValue = value;
                if (_constantValue == AppConsts.CURRENT_DAY || _constantValue == AppConsts.CURRENT_MONTH
                    || _constantValue == AppConsts.CURRENT_YEAR)
                {
                    CurrentChecked = true;
                }
                else if (ddlConstantType.SelectedValue == BkgConstantType.CITY.GetStringValue()
                    || ddlConstantType.SelectedValue == BkgConstantType.COUNTY.GetStringValue()
                    || ddlConstantType.SelectedValue == BkgConstantType.STATE.GetStringValue()
                    || ddlConstantType.SelectedValue == BkgConstantType.COUNTRY.GetStringValue())
                {
                    SelectedConstantValue = _constantValue;
                }
                else
                {
                    SelectedConstantText = _constantValue;
                }
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

        public String SelectedConstantValue
        {
            get
            {
                return ddlConstantData.SelectedValue;
            }
            set
            {
                ddlConstantData.SelectedValue = value;
            }
        }

        public String SelectedConstantText
        {
            get
            {
                return txtConstant.Text;
            }
            set
            {
                txtConstant.Text = value;
            }
        }

        public Int32? constantTypeId
        {
            get { return _constantTypeId; }
            set
            {
                _constantTypeId = value;
            }
        }

        public String SelectedServiceName
        {
            get
            {
                return ddlServices.SelectedText;
            }
        }

        public String SelectedAttributeName
        {
            get
            {
                return ddlAttribute.SelectedText;
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
                    Presenter.OnViewInitialized();

                }
                Presenter.OnViewLoaded();
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

        #region Drop Down Events

        protected void ddlObjectType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                BindControlsOnTheBasisOfObjectType();
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

        protected void ddlAttributegrp_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                if (ddlAttributegrp.SelectedValue != String.Empty)
                {
                    BindAttributeList();
                    SelectedAttributeId = AppConsts.NONE;
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

        protected void ddlConstantGroup_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                BindConstantType();
                ddlConstantType.SelectedValue = AppConsts.ZERO;
                ddlConstantData.Visible = false;
                chkCurrent.Visible = false;
                txtConstant.Visible = false;
                txtConstant.Text = String.Empty;
                chkCurrent.Checked = false;
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

        protected void ddlConstantType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                BindDataForConstant();
                txtConstant.Text = String.Empty;
                chkCurrent.Checked = false;
                //Changes w.r.t. UAT 627 - Fill in "United States" as default selection for Country dropdowns.
                //ddlConstantData.SelectedValue = AppConsts.ZERO;
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

        protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ShowHideTextBoxOnCheckBoxChange();
                txtConstant.Text = String.Empty;
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

        #region Private Methods

        private void AddDefaultItem(INTERSOFT.WEB.UI.WebControls.WclDropDownList ddTarget, String textToInsert)
        {
            ddTarget.Items.Insert(0, new DropDownListItem { Text = textToInsert, Value = AppConsts.ZERO });
        }

        private void BindControlsOnTheBasisOfObjectType()
        {
            if (SelectedRuleObjectTypeCode == BkgRuleObjectMappingType.SERVICE_RESULT.GetStringValue())
            {
                ShowHideControls(true, false, false);
                BindServices();
            }
            else if (SelectedRuleObjectTypeCode == BkgRuleObjectMappingType.DATA_VALUE.GetStringValue())
            {
                ShowHideControls(false, true, false);
                BindAttribteGroup();
            }
            else if (SelectedRuleObjectTypeCode == BkgRuleObjectMappingType.DEFINED_VALUE.GetStringValue())
            {
                ShowHideControls(false, false, true);
                BindConstantGroup();
            }
            else
            {
                ShowHideControls(false, false, false);
            }
        }

        private void ShowHideControls(Boolean srvcRsltVsbl, Boolean atrbtVsbl, Boolean cnstntVsbl)
        {
            ddlServices.Visible = srvcRsltVsbl;
            ddlAttributegrp.Visible = atrbtVsbl;
            ddlAttribute.Visible = atrbtVsbl;
            ddlConstantGroup.Visible = cnstntVsbl;
            ddlConstantType.Visible = cnstntVsbl;
            ddlConstantData.Visible = false;
            chkCurrent.Visible = false;
            txtConstant.Visible = false;
            ddlServices.SelectedValue = AppConsts.ZERO;
            ddlAttributegrp.SelectedValue = AppConsts.ZERO;
            ddlAttribute.SelectedValue = AppConsts.ZERO;
            ddlConstantGroup.SelectedValue = AppConsts.ZERO;
            ddlConstantType.SelectedValue = AppConsts.ZERO;
            ddlConstantData.SelectedValue = AppConsts.ZERO;
        }

        private void BindServices()
        {
            lstBkgPackageSvc = bkgDataStore.getServicesByPackageId(PackageId, SelectedTenantId);
            ddlServices.DataSource = lstBkgPackageSvc;
            ddlServices.DataValueField = "BkgPackageSrvcId";
            ddlServices.DataTextField = "ServiceName";
            ddlServices.DataBind();
            AddDefaultItem(ddlServices, "-- SELECT --");
            if (_selectedServiceId != 0)
            {
                SelectedServiceId = _selectedServiceId;
            }
        }

        private void BindAttribteGroup()
        {
            attributeList = bkgDataStore.getAttributeList(PackageId, SelectedTenantId);
            ddlAttributegrp.DataSource = attributeList.DistinctBy(x => x.BkgAttributeGroupId).OrderBy(col => col.BkgAttributeGroupName).ToList();
            ddlAttributegrp.DataTextField = "BkgAttributeGroupName";
            ddlAttributegrp.DataValueField = "BkgAttributeGroupId";
            ddlAttributegrp.DataBind();
            AddDefaultItem(ddlAttributegrp, "-- SELECT --");
            AddDefaultItem(ddlAttribute, "-- SELECT --");
            if (this._attributeGrpMappingId != AppConsts.NONE)
            {
                var selectedAttribute = attributeList.FirstOrDefault(x => x.BkgAttributeGroupMappingId == _attributeGrpMappingId);
                SelectedAttributeId = selectedAttribute.BkgAttributeId;
                SelectedAttributeGrpId = selectedAttribute.BkgAttributeGroupId;
                BindAttributeList();
            }
        }

        private void BindAttributeList()
        {
            attributeList = bkgDataStore.getAttributeList(PackageId, SelectedTenantId);
            ddlAttribute.DataSource = attributeList.Where(x => x.BkgAttributeGroupId == SelectedAttributeGrpId).ToList();
            ddlAttribute.DataTextField = "BkgAttributeName";
            ddlAttribute.DataValueField = "BkgAttributeId";
            ddlAttribute.DataBind();
            AddDefaultItem(ddlAttribute, "-- SELECT --");
        }

        private void BindConstantGroup()
        {
            lstConstantGroup = bkgDataStore.getConstantType(SelectedTenantId);
            ddlConstantGroup.DataSource = lstConstantGroup.GroupBy(x => x.BCT_Group).Distinct().ToList();
            ddlConstantGroup.DataTextField = "Key";
            ddlConstantGroup.DataValueField = "Key";
            ddlConstantGroup.DataBind();
            AddDefaultItem(ddlConstantGroup, "-- SELECT GROUP --");
            AddDefaultItem(ddlConstantType, "-- SELECT CONSTANT TYPE --");
            if (_constantTypeId != null)
            {
                lkpBkgConstantType constantType = lstConstantGroup.FirstOrDefault(x => x.BCT_ID == _constantTypeId);
                SelectedConstatGroupValue = constantType.BCT_Group;
                SelectedConstantTypeCode = constantType.BCT_Code;
                if (SelectedConstantTypeCode != String.Empty)
                {
                    BindConstantType();
                    BindDataForConstant();
                }
            }
        }

        private void BindConstantType()
        {
            lstConstantType = bkgDataStore.getConstantTypeByGroup(SelectedConstatGroupValue, SelectedTenantId);
            ddlConstantType.DataSource = lstConstantType;
            ddlConstantType.DataTextField = "BCT_Name";
            ddlConstantType.DataValueField = "BCT_Code";
            ddlConstantType.DataBind();
            AddDefaultItem(ddlConstantType, "-- SELECT CONSTANT TYPE --");
        }

        private void BindConstantData()
        {
            Dictionary<String, String> constantData = new Dictionary<String, String>();
            if (ddlConstantType.SelectedValue != AppConsts.ZERO)
                constantData = bkgDataStore.getConstantData(ddlConstantType.SelectedValue);
            ddlConstantData.DataSource = constantData.OrderBy(col=>col.Value);
            ddlConstantData.DataValueField = "key";
            ddlConstantData.DataTextField = "value";
            ddlConstantData.DataBind();
            AddDefaultItem(ddlConstantData, "-- SELECT -- ");
            if (!_constantValue.IsNullOrEmpty() && _constantValue != AppConsts.ZERO)
            {
                ddlConstantData.SelectedValue = _constantValue;
            }
            //Changes w.r.t. UAT 627 - Fill in "United States" as default selection for Country dropdowns. 
            else if (constantData.ContainsKey("UNITED STATES"))
            {
                ddlConstantData.SelectedValue = constantData.FirstOrDefault(x => x.Value.Contains("UNITED STATES")).Key;
            }
        }

        private void BindDataForConstant()
        {
            if (ddlConstantType.SelectedValue == BkgConstantType.EMPTY.GetStringValue()
                || ddlConstantType.SelectedValue == String.Empty)
            {
                ShowHideCostantControls(false, false, false);

            }
            else if (ddlConstantType.SelectedValue == BkgConstantType.DAY.GetStringValue()
                 || ddlConstantType.SelectedValue == BkgConstantType.MONTH.GetStringValue()
                 || ddlConstantType.SelectedValue == BkgConstantType.YEAR.GetStringValue())
            {
                chkCurrent.AutoPostBack = true;
                ShowHideCostantControls(true, true, false);
                if (_constantValue == AppConsts.CURRENT_DAY || _constantValue == AppConsts.CURRENT_MONTH
                 || _constantValue == AppConsts.CURRENT_YEAR || _constantValue == AppConsts.EMPTY)
                {
                    txtConstant.Visible = false;
                }
            }
            else if (ddlConstantType.SelectedValue == BkgConstantType.DATE.GetStringValue()
                 || ddlConstantType.SelectedValue == BkgConstantType.NUMEIC.GetStringValue()
                 || ddlConstantType.SelectedValue == BkgConstantType.TEXT.GetStringValue()
                || ddlConstantType.SelectedValue == BkgConstantType.BOOL.GetStringValue())
            {
                ShowHideCostantControls(true, false, false);
            }
            else
            {
                ShowHideCostantControls(false, false, true);
            }
        }

        private void ShowHideCostantControls(Boolean txtVisiblity, Boolean chkCurrentVisiblity, Boolean ddlCnstntVisiblity)
        {
            txtConstant.Visible = txtVisiblity;
            chkCurrent.Visible = chkCurrentVisiblity;
            ddlConstantData.Visible = ddlCnstntVisiblity;
            if (ddlConstantData.Visible)
            {
                BindConstantData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowHideTextBoxOnCheckBoxChange()
        {
            if (CurrentChecked)
            {
                txtConstant.Visible = false;
            }
            else
            {
                txtConstant.Visible = true;
            }
        }

        public Boolean IsExpressionObjectIsValid()
        {
            if (ddlObjectType.SelectedValue == AppConsts.ZERO)
                return false;
            else
            {
                if (ddlServices.Visible == true)
                {
                    if (ddlServices.SelectedValue == AppConsts.ZERO)
                        return false;
                }
                else if (ddlAttributegrp.Visible == true)
                {
                    if (ddlAttributegrp.SelectedValue == AppConsts.ZERO || ddlAttribute.SelectedValue == AppConsts.ZERO)
                        return false;
                }
                else if (ddlConstantGroup.Visible == true)
                {
                    if (ddlConstantGroup.SelectedValue == AppConsts.ZERO || ddlConstantType.SelectedValue == AppConsts.ZERO)
                        return false;
                    if (txtConstant.Visible)
                    {
                        if (txtConstant.Text == String.Empty)
                            return false;
                    }
                    if (ddlConstantData.Visible)
                    {
                        if (ddlConstantData.SelectedValue == AppConsts.ZERO)
                            return false;
                    }
                }
            }
            return true;
        }

        #endregion
    }
}