using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Collections.Generic;
using Telerik.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Web.UI;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.UI.HtmlControls;
using INTERSOFT.WEB.UI.WebControls;
using System.Xml;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ShotSeriesShuffleRuleTest : BaseUserControl, IShotSeriesShuffleRuleTestView
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ShotSeriesShuffleRuleTestPresenter _presenter = new ShotSeriesShuffleRuleTestPresenter();
        private String _viewType;
        #endregion
        #endregion

        #region properties

        public ShotSeriesShuffleRuleTestPresenter Presenter
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

        public IShotSeriesShuffleRuleTestView CurrentViewContext
        {
            get { return this; }
        }


        public String ErrorMessage
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

        public List<RuleDetailsForTestContract> LstSeriesRuleDetails
        {
            get
            {
                if (ViewState["LstSeriesRuleDetails"].IsNullOrEmpty())
                {
                    return new List<RuleDetailsForTestContract>();
                }
                return ViewState["LstSeriesRuleDetails"] as List<RuleDetailsForTestContract>;
            }
            set
            {
                ViewState["LstSeriesRuleDetails"] = value;
            }
        }

        public List<lkpItemComplianceStatu> LstItemComplianceStatus
        {
            get
            {
                if (ViewState["LstItemComplianceStatus"].IsNullOrEmpty())
                {
                    return new List<lkpItemComplianceStatu>();
                }
                return ViewState["LstItemComplianceStatus"] as List<lkpItemComplianceStatu>;
            }
            set
            {
                ViewState["LstItemComplianceStatus"] = value;
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
                    BindSeriesRuleDetails();
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

        #region Repeater Events

        protected void rptShotSeriesData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                RadComboBox ddlItemStatus = (RadComboBox)e.Item.FindControl("ddlItemStatus");
                HtmlGenericControl divObjectName = (HtmlGenericControl)e.Item.FindControl("divObjectName");
                HtmlGenericControl divAttribute = (HtmlGenericControl)e.Item.FindControl("divAttribute");
                HtmlGenericControl divItem = (HtmlGenericControl)e.Item.FindControl("divItem");
                HtmlGenericControl divConstant = (HtmlGenericControl)e.Item.FindControl("divConstant");
                Label lblConstant = (Label)e.Item.FindControl("lblConstant");
                Label lbObjectName = (Label)e.Item.FindControl("lbObjectName");


                RuleDetailsForTestContract seriesRuleDetailsForShuffleTestContract = (RuleDetailsForTestContract)e.Item.DataItem;
                if (!seriesRuleDetailsForShuffleTestContract.IsNullOrEmpty())
                {
                    if (seriesRuleDetailsForShuffleTestContract.ObjectMappingTypeCode == ObjectMappingType.Defined_Value.GetStringValue())
                    {
                        //divConstant.Visible = true;
                        divObjectName.Visible = false;
                        lblConstant.Text = seriesRuleDetailsForShuffleTestContract.ConstantValue;
                        lbObjectName.Text = "Constant";
                    }
                    else if (seriesRuleDetailsForShuffleTestContract.ObjectMappingTypeCode == ObjectMappingType.Compliance_Value.GetStringValue())
                    {
                        divItem.Visible = true;
                        ddlItemStatus.DataSource = LstItemComplianceStatus;
                        ddlItemStatus.DataBind();
                        lbObjectName.Text = seriesRuleDetailsForShuffleTestContract.CategoryName + " > " + seriesRuleDetailsForShuffleTestContract.ItemName;
                    }
                    else if (seriesRuleDetailsForShuffleTestContract.ObjectMappingTypeCode == ObjectMappingType.Data_Value.GetStringValue())
                    {
                        divAttribute.Visible = true;
                        lbObjectName.Text = seriesRuleDetailsForShuffleTestContract.CategoryName
                                                                + " > " + seriesRuleDetailsForShuffleTestContract.ItemName
                                                                + " > " + seriesRuleDetailsForShuffleTestContract.AttributeName;

                        HandleAttributeDisplay(e, seriesRuleDetailsForShuffleTestContract);
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

        #endregion

        #region Methods

        #region Private Methods

        public void BindSeriesRuleDetails()
        {
            if (LstSeriesRuleDetails.IsNullOrEmpty())
            {
                divDetails.Visible = false;
            }
            else
            {
                List<RuleDetailsForTestContract> lstConstantRules = LstSeriesRuleDetails
                                                        .Where(con => con.ObjectMappingTypeCode.ToLower() == ObjectMappingType.Defined_Value.GetStringValue().ToLower()).ToList();
                List<RuleDetailsForTestContract> lstDataAndComplianceValues = LstSeriesRuleDetails
                                                                             .Where(con => con.ObjectMappingTypeCode.ToLower() != ObjectMappingType.Defined_Value.GetStringValue().ToLower())
                                                                                    .GroupBy(cond => new
                                                                                {
                                                                                    cond.ComplianceCategoryID,
                                                                                    cond.ComplianceItemID,
                                                                                    cond.ComplianceAttributeID
                                                                                })
                                                                                .Select(col => col.First())
                                                                                .ToList();
                if (!lstDataAndComplianceValues.IsNullOrEmpty())
                {
                    divDetails.Visible = true;
                }
                else
                {
                    divDetails.Visible = false;
                }
                rptShotSeriesData.DataSource = lstConstantRules.Union(lstDataAndComplianceValues)
                                                                    .OrderBy(col => col.RuleMappingDetailID)
                                                                    .ToList();
                rptShotSeriesData.DataBind();
            }
        }

        private void HandleAttributeDisplay(RepeaterItemEventArgs e, RuleDetailsForTestContract seriesRuleDetailsForShuffleTestContract)
        {
            if (seriesRuleDetailsForShuffleTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dtPicker = (WclDatePicker)e.Item.FindControl("dtPicker");
                dtPicker.Visible = true;
            }
            else if (seriesRuleDetailsForShuffleTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtBox = (WclTextBox)e.Item.FindControl("txtBox");
                txtBox.Visible = true;
            }
            else if (seriesRuleDetailsForShuffleTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim()
              || seriesRuleDetailsForShuffleTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower().Trim()
                || seriesRuleDetailsForShuffleTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim())
            {
                WclNumericTextBox numericTextBox = (WclNumericTextBox)e.Item.FindControl("numericTextBox");
                numericTextBox.Visible = true;
            }
            else if (seriesRuleDetailsForShuffleTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
            {
                String _optionsCode = ComplianceAttributeDatatypes.Options.GetStringValue();

                var _cmbData = LstSeriesRuleDetails
                               .Where(attrOption => attrOption.ComplianceAttributeID == seriesRuleDetailsForShuffleTestContract.ComplianceAttributeID
                                && attrOption.ComplianceAttributeDataTypeCode == _optionsCode && attrOption.RuleMappingID == seriesRuleDetailsForShuffleTestContract.RuleMappingID)
                                .Distinct()
                               .ToList().Select(optn => new { optn.OptionText, optn.OptionValue });
                WclComboBox optionCombo = (WclComboBox)e.Item.FindControl("optionCombo");
                optionCombo.Visible = true;
                foreach (var attributeOption in _cmbData)
                    optionCombo.Items.Add(new RadComboBoxItem(attributeOption.OptionText, attributeOption.OptionValue));
                optionCombo.Items.Insert(0, new RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
                optionCombo.DataBind();
            }
        }

        private String GetRuleValue(RepeaterItem item)
        {
            String value = String.Empty;
            Label lblObjectMappingTypeCode = (Label)item.FindControl("lblObjectMappingTypeCode");
            String objectMappingTypeCode = lblObjectMappingTypeCode.Text.Trim();
            if (objectMappingTypeCode == ObjectMappingType.Defined_Value.GetStringValue())
            {
                value = (item.FindControl("lblConstant") as Label).Text.Trim();
            }
            else if (objectMappingTypeCode == ObjectMappingType.Compliance_Value.GetStringValue())
            {

                value = (item.FindControl("ddlItemStatus") as WclComboBox).SelectedValue;
            }
            else if (objectMappingTypeCode == ObjectMappingType.Data_Value.GetStringValue())
            {
                String complianceAttributeDataTypeCode = (item.FindControl("lblComplianceAttributeDataTypeCode") as Label).Text.Trim();
                if (complianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
                {
                    WclDatePicker dtPicker = (WclDatePicker)item.FindControl("dtPicker");
                    value = Convert.ToString(dtPicker.SelectedDate);
                }
                else if (complianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
                {
                    WclTextBox txtBox = (WclTextBox)item.FindControl("txtBox");
                    value = txtBox.Text;
                }
                else if (complianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim()
                  || complianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower().Trim()
                    || complianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim())
                {
                    WclNumericTextBox numericTextBox = (WclNumericTextBox)item.FindControl("numericTextBox");
                    value = numericTextBox.Text;
                }
                else if (complianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
                {
                    WclComboBox optionCombo = (WclComboBox)item.FindControl("optionCombo");
                    value = optionCombo.SelectedValue;
                }
            }
            return value;
        }

        #endregion

        #region Public Methods

        public XmlDocument GetShotSeriesRuleData()
        {

            XmlDocument doc = new XmlDocument();
            XmlElement rootNode = (XmlElement)doc.AppendChild(doc.CreateElement("RuleMappings"));
            foreach (RepeaterItem item in rptShotSeriesData.Items)
            {

                Label lblRuleMappingDetailID = (Label)item.FindControl("lblRuleMappingDetailID");
                Label lblObjectMappingTypeCode = (Label)item.FindControl("lblObjectMappingTypeCode");
                String objectMappingTypeCode = lblObjectMappingTypeCode.Text.Trim();
                String value = GetRuleValue(item);
                Int32 ruleMappingDetailID = Convert.ToInt32(lblRuleMappingDetailID.Text.Trim());
                if (objectMappingTypeCode == ObjectMappingType.Defined_Value.GetStringValue())
                {
                    XmlNode ruleMappingDetailNode = rootNode.AppendChild(doc.CreateElement("RuleMapping"));
                    ruleMappingDetailNode.AppendChild(doc.CreateElement("Id")).InnerText = ruleMappingDetailID.ToString();
                    ruleMappingDetailNode.AppendChild(doc.CreateElement("Value")).InnerText = value;
                }
                else if (objectMappingTypeCode == ObjectMappingType.Compliance_Value.GetStringValue())
                {
                    RuleDetailsForTestContract currentRuleDetailsContract = LstSeriesRuleDetails
                            .Where(cond => cond.RuleMappingDetailID == ruleMappingDetailID && cond.ObjectMappingTypeCode.ToLower() == objectMappingTypeCode.ToLower())
                            .FirstOrDefault();
                    List<RuleDetailsForTestContract> currentRuleDetailsContractList = LstSeriesRuleDetails
                            .Where(cond => cond.ObjectMappingTypeCode.ToLower() == objectMappingTypeCode.ToLower()
                            && cond.ComplianceCategoryID == currentRuleDetailsContract.ComplianceCategoryID
                            && cond.ComplianceItemID == currentRuleDetailsContract.ComplianceItemID)
                            .ToList();
                    foreach (RuleDetailsForTestContract currentRuleDetail in currentRuleDetailsContractList)
                    {
                        XmlNode ruleMappingDetailNode = rootNode.AppendChild(doc.CreateElement("RuleMapping"));
                        ruleMappingDetailNode.AppendChild(doc.CreateElement("Id")).InnerText = currentRuleDetail.RuleMappingDetailID.ToString();
                        ruleMappingDetailNode.AppendChild(doc.CreateElement("Value")).InnerText = value;
                    }
                }
                else if (objectMappingTypeCode == ObjectMappingType.Data_Value.GetStringValue())
                {
                    RuleDetailsForTestContract currentRuleDetailsContract = LstSeriesRuleDetails
                            .Where(cond => cond.RuleMappingDetailID == ruleMappingDetailID && cond.ObjectMappingTypeCode.ToLower() == objectMappingTypeCode.ToLower())
                            .FirstOrDefault();
                    List<RuleDetailsForTestContract> currentRuleDetailsContractList = LstSeriesRuleDetails
                            .Where(cond => cond.ObjectMappingTypeCode.ToLower() == objectMappingTypeCode.ToLower()
                            && cond.ComplianceCategoryID == currentRuleDetailsContract.ComplianceCategoryID
                            && cond.ComplianceItemID == currentRuleDetailsContract.ComplianceItemID
                            && cond.ComplianceAttributeID == currentRuleDetailsContract.ComplianceAttributeID)
                            .GroupBy(col => col.RuleMappingDetailID)
                            .Select(col => col.First())
                            .ToList();
                    foreach (RuleDetailsForTestContract currentRuleDetail in currentRuleDetailsContractList)
                    {
                        XmlNode ruleMappingDetailNode = rootNode.AppendChild(doc.CreateElement("RuleMapping"));
                        ruleMappingDetailNode.AppendChild(doc.CreateElement("Id")).InnerText = currentRuleDetail.RuleMappingDetailID.ToString();
                        ruleMappingDetailNode.AppendChild(doc.CreateElement("Value")).InnerText = value;
                    }
                }
            }
            return doc;
        }

        #endregion

        #endregion
    }
}
