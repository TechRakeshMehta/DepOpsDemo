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
using System.Text;
using System.Drawing;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ComplianceRuleTest : BaseWebPage, IComplianceRuleTestView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ComplianceRuleTestPersenter _presenter = new ComplianceRuleTestPersenter();
        private String _viewType;
        #endregion
        #endregion

        #region properties

        public ComplianceRuleTestPersenter Presenter
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

        public IComplianceRuleTestView CurrentViewContext
        {
            get { return this; }
        }


        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 IComplianceRuleTestView.SelectedTenantId
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

        List<RuleDetailsForTestContract> IComplianceRuleTestView.ComplianceRuleData { get; set; }

        List<lkpItemComplianceStatu> IComplianceRuleTestView.LstItemComplianceStatus
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

        Int32 IComplianceRuleTestView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IComplianceRuleTestView.RuleMappingID
        {
            get
            {
                return Convert.ToInt32(ViewState["RuleMappingID"]);
            }
            set
            {
                ViewState["RuleMappingID"] = value;
            }
        }

        Int32 IComplianceRuleTestView.RuleActionTypeID
        {
            get
            {
                return Convert.ToInt32(ViewState["RuleActionTypeID"]);
            }
            set
            {
                ViewState["RuleActionTypeID"] = value;
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
                    Page.Title = "Compliance Rule Test";
                    Presenter.OnViewInitialized();
                    CurrentViewContext.RuleMappingID = Convert.ToInt32(Request.QueryString["RuleMappingID"]);
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    CurrentViewContext.RuleActionTypeID = Convert.ToInt32(Request.QueryString["RuleActionTypeID"]);
                    BindRuleDetails();
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

        protected void rptComplianceRuleData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                String incompleteStatusCode = ApplicantItemComplianceStatus.Incomplete.GetStringValue();
                Int32 incompleteStatusID = CurrentViewContext.LstItemComplianceStatus.Where(col => col.Code == incompleteStatusCode).FirstOrDefault().ItemComplianceStatusID;

                RadComboBox ddlItemStatus = (RadComboBox)e.Item.FindControl("ddlItemStatus");
                HtmlGenericControl divObjectName = (HtmlGenericControl)e.Item.FindControl("divObjectName");
                HtmlGenericControl divAttribute = (HtmlGenericControl)e.Item.FindControl("divAttribute");
                HtmlGenericControl divItem = (HtmlGenericControl)e.Item.FindControl("divItem");
                HtmlGenericControl divConstant = (HtmlGenericControl)e.Item.FindControl("divConstant");
                Label lblConstant = (Label)e.Item.FindControl("lblConstant");
                Label lbObjectName = (Label)e.Item.FindControl("lbObjectName");


                RuleDetailsForTestContract ruleDetailsForTestContract = (RuleDetailsForTestContract)e.Item.DataItem;
                if (!ruleDetailsForTestContract.IsNullOrEmpty())
                {
                    if (ruleDetailsForTestContract.ObjectMappingTypeCode == ObjectMappingType.Defined_Value.GetStringValue())
                    {
                        divObjectName.Visible = false;
                        lblConstant.Text = ruleDetailsForTestContract.ConstantValue.HtmlEncode();
                        lbObjectName.Text = "Constant";
                    }
                    else if (ruleDetailsForTestContract.ObjectMappingTypeCode == ObjectMappingType.Compliance_Value.GetStringValue())
                    {
                        divItem.Visible = true;
                        ddlItemStatus.DataSource = CurrentViewContext.LstItemComplianceStatus;
                        ddlItemStatus.DataBind();
                        ddlItemStatus.SelectedValue = incompleteStatusID.ToString();
                        lbObjectName.Text = ruleDetailsForTestContract.CategoryName + " > " + ruleDetailsForTestContract.ItemName;
                    }
                    else if (ruleDetailsForTestContract.ObjectMappingTypeCode == ObjectMappingType.Data_Value.GetStringValue())
                    {
                        divAttribute.Visible = true;
                        lbObjectName.Text = ruleDetailsForTestContract.CategoryName
                                                                + " > " + ruleDetailsForTestContract.ItemName
                                                                + " > " + ruleDetailsForTestContract.AttributeName;

                        HandleAttributeDisplay(e, ruleDetailsForTestContract);
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

        protected void cmdbarTestRule_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc = GetRuleData();
                dvRuleResult.Visible = true;
                String resultXml = Presenter.TestComplianceRule(doc.OuterXml);
                if (Presenter.IsActionTypeDueDate()) //UAT-2740
                {
                    String resultDateXML = Presenter.CalculateDueDate(resultXml);
                    resultXml = resultDateXML;
                }
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(resultXml);
                XmlNode nodeResult = xml.SelectSingleNode("Results/Result");
                if (!nodeResult.IsNullOrEmpty())
                {
                    Boolean isSuccess;
                    //Boolean isError = Convert.ToBoolean(Convert.ToInt32(nodeResult["Status"].InnerText));
                    if (nodeResult["ErrorStackTrace"].InnerText == String.Empty)
                    {
                        isSuccess = nodeResult["Result"].InnerText.ToString().ToLower() == "false" ? false : true;
                        lblResultText.Text = nodeResult["Result"].InnerText;
                        lblExpressionText.Text = nodeResult["Expression"].InnerText;
                        if (!isSuccess)
                        {
                            lblMessageText.Text = nodeResult["ErrorMessage"].InnerText;
                            lblMessageText.ForeColor = Color.Red;
                        }
                        else
                        {
                            lblMessageText.Text = nodeResult["SuccessMessage"].InnerText;
                            lblMessageText.ForeColor = Color.Green;
                        }
                    }
                    else
                    {
                        lblMessageText.Text = "Something went wrong. Please check setup and input data.";
                        lblMessageText.ForeColor = Color.Red;
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

        private void BindRuleDetails()
        {
            Presenter.GetComplianceRuleData();
            lblRuleExpression.Text = CurrentViewContext.ComplianceRuleData.FirstOrDefault().UIExpression.HtmlEncode();
            rptComplianceRuleData.DataSource = CurrentViewContext.ComplianceRuleData.Distinct()
                                                                    .GroupBy(col => col.RuleMappingDetailID)
                                                                    .Select(col => col.First()).ToList();
            rptComplianceRuleData.DataBind();
        }

        private void HandleAttributeDisplay(RepeaterItemEventArgs e, RuleDetailsForTestContract ruleDetailsForTestContract)
        {
            if (ruleDetailsForTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dtPicker = (WclDatePicker)e.Item.FindControl("dtPicker");
                dtPicker.Visible = true;
            }
            else if (ruleDetailsForTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtBox = (WclTextBox)e.Item.FindControl("txtBox");
                txtBox.Visible = true;
            }
            else if (ruleDetailsForTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim()
              || ruleDetailsForTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower().Trim()
                || ruleDetailsForTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim())
            {
                WclNumericTextBox numericTextBox = (WclNumericTextBox)e.Item.FindControl("numericTextBox");
                numericTextBox.Visible = true;
            }
            else if (ruleDetailsForTestContract.ComplianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
            {
                String _optionsCode = ComplianceAttributeDatatypes.Options.GetStringValue();

                var _cmbData = CurrentViewContext.ComplianceRuleData
                               .Where(attrOption => attrOption.ComplianceAttributeID == ruleDetailsForTestContract.ComplianceAttributeID
                                && attrOption.ComplianceAttributeDataTypeCode == _optionsCode && attrOption.RuleMappingID == ruleDetailsForTestContract.RuleMappingID)
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
            String approvedStatusCode = ApplicantItemComplianceStatus.Approved.GetStringValue();
            Int32 approvedStatusID = CurrentViewContext.LstItemComplianceStatus.Where(col => col.Code == approvedStatusCode).FirstOrDefault().ItemComplianceStatusID;

            String value = String.Empty;
            Label lblObjectMappingTypeCode = (Label)item.FindControl("lblObjectMappingTypeCode");
            String objectMappingTypeCode = lblObjectMappingTypeCode.Text.Trim();
            if (objectMappingTypeCode == ObjectMappingType.Defined_Value.GetStringValue())
            {
                value = (item.FindControl("lblConstant") as Label).Text.Trim();
            }
            else if (objectMappingTypeCode == ObjectMappingType.Compliance_Value.GetStringValue())
            {
                Int32 itemStatusId = Convert.ToInt32((item.FindControl("ddlItemStatus") as WclComboBox).SelectedValue);
                value = itemStatusId == approvedStatusID ? true.ToString() : false.ToString();
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

        private XmlDocument GetRuleData()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement rootNode = (XmlElement)doc.AppendChild(doc.CreateElement("Rules"));
            XmlElement elementRule = (XmlElement)rootNode.AppendChild(doc.CreateElement("Rule"));
            elementRule.AppendChild(doc.CreateElement("Id")).InnerText = CurrentViewContext.RuleMappingID.ToString();
            XmlNode objectMappingNode = elementRule.AppendChild(doc.CreateElement("ObjectMapping"));
            XmlNode mappingsNode = objectMappingNode.AppendChild(doc.CreateElement("Mappings"));
            foreach (RepeaterItem item in rptComplianceRuleData.Items)
            {
                XmlNode mappingNode = mappingsNode.AppendChild(doc.CreateElement("Mapping"));
                Label lblPlaceHolderName = (Label)item.FindControl("lblPlaceHolderName");
                mappingNode.AppendChild(doc.CreateElement("Key")).InnerText = lblPlaceHolderName.Text.Trim();
                String value = GetRuleValue(item);
                mappingNode.AppendChild(doc.CreateElement("Value")).InnerText = value;
            }
            return doc;
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}