using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using Telerik.Web.UI;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Linq;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ShotSeriesShuffleTest : BaseWebPage, IShotSeriesShuffleTestView
    {
        #region Variables

        private ShotSeriesShuffleTestPresenter _presenter = new ShotSeriesShuffleTestPresenter();

        #endregion

        #region Properties


        public ShotSeriesShuffleTestPresenter Presenter
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

        private Int32 _tenantid;

        /// <summary>
        /// Get or Set the Tenant ID
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
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

        public IShotSeriesShuffleTestView CurrentViewContext
        {
            get { return this; }
        }

        List<SeriesAttributeContract> IShotSeriesShuffleTestView.SeriesData
        {
            get
            {
                if (ViewState["SeriesData"].IsNullOrEmpty())
                {
                    return new List<SeriesAttributeContract>();
                }
                return ViewState["SeriesData"] as List<SeriesAttributeContract>;
            }
            set
            {
                ViewState["SeriesData"] = value;
            }
        }

        Int32 IShotSeriesShuffleTestView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IShotSeriesShuffleTestView.CurrentSeriesID
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentSeriesID"]);
            }
            set
            {
                ViewState["CurrentSeriesID"] = value;
            }
        }

        String IShotSeriesShuffleTestView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 IShotSeriesShuffleTestView.SelectedTenantId
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

        Int32 IShotSeriesShuffleTestView.DefaultTenantId
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

        List<lkpItemComplianceStatu> IShotSeriesShuffleTestView.LstItemComplianceStatus
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
        List<RuleDetailsForTestContract> IShotSeriesShuffleTestView.LstSeriesRuleDetails
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
        List<SeriesAttributeContract> IShotSeriesShuffleTestView.SeriesDataAfterShuffle { get; set; }
        ShotSeriesSaveResponse IShotSeriesShuffleTestView.ShotSeriesResponse { get; set; }
        Int32 IShotSeriesShuffleTestView.CategoryID
        {
            get
            {
                return Convert.ToInt32(ViewState["CategoryID"]);
            }
            set
            {
                ViewState["CategoryID"] = value;
            }
        }

        List<CompliancePackage> IShotSeriesShuffleTestView.LstPackages { get; set; }

        Int32 IShotSeriesShuffleTestView.SelectedPackageID
        {
            get
            {
                if (!cmbPackage.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbPackage.SelectedValue);
                }
                return AppConsts.NONE;
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
                    Page.Title = "Shot Series Shuffle Test";
                    Presenter.OnViewInitialized();
                    CurrentViewContext.CurrentSeriesID = Convert.ToInt32(Request.QueryString["Id"]);
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    CurrentViewContext.CategoryID = Convert.ToInt32(Request.QueryString["CatId"]);
                    BindSeriesDetails();
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

        #region Button Events

        protected void cmdbarShuffle_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement rootNode = (XmlElement)doc.AppendChild(doc.CreateElement("SeriesItems"));
                rootNode.AppendChild(doc.CreateElement("ItemSeriesID")).InnerText = Convert.ToString(CurrentViewContext.CurrentSeriesID);
                foreach (RepeaterItem item in rptShotSeriesData.Items)
                {
                    XmlNode itemNode = rootNode.AppendChild(doc.CreateElement("SeriesItem"));
                    Label lblItemSeriesItemID = (Label)item.FindControl("lblItemSeriesItemID");
                    itemNode.AppendChild(doc.CreateElement("Id")).InnerText = lblItemSeriesItemID.Text.Trim();
                    XmlNode attributeNode = itemNode.AppendChild(doc.CreateElement("Attributes"));
                    String keyAttributeValue = String.Empty;
                    Repeater rptColumn = (Repeater)item.FindControl("rptColumn");
                    if (rptColumn.IsNotNull())
                    {
                        foreach (RepeaterItem attributeRepeaterItem in rptColumn.Items)
                        {
                            XmlNode individaulattributeNode = attributeNode.AppendChild(doc.CreateElement("Attribute"));
                            Int32 itemSeriesAttributeID = Convert.ToInt32(((Label)attributeRepeaterItem.FindControl("lblItemSeriesAttributeID")).Text.Trim());
                            individaulattributeNode.AppendChild(doc.CreateElement("Id")).InnerText = itemSeriesAttributeID.ToString();
                            SeriesAttributeContract currentAttribute = CurrentViewContext.SeriesData.FirstOrDefault(cond => cond.ItemSeriesAttributeId == itemSeriesAttributeID);
                            String value = GetAttributeValue(attributeRepeaterItem, currentAttribute.CmpAttributeDatatypeCode);
                            individaulattributeNode.AppendChild(doc.CreateElement("Value")).InnerText = value;
                            if (currentAttribute.IsKeyAttribute)
                            {
                                keyAttributeValue = value;
                            }
                        }
                    }
                    itemNode.AppendChild(doc.CreateElement("Value")).InnerText = keyAttributeValue;
                    WclComboBox ddlItemStatus = (WclComboBox)item.FindControl("ddlItemStatus");
                    itemNode.AppendChild(doc.CreateElement("StatusId")).InnerText = ddlItemStatus.SelectedValue;
                }

                XmlDocument ruleXmlDoc = new XmlDocument();
                if (!CurrentViewContext.LstSeriesRuleDetails.IsNullOrEmpty())
                {
                    ruleXmlDoc = ShotSeriesShuffleRuleTest.GetShotSeriesRuleData();
                }
                Presenter.GetSeriesDetailsAfterShuffleTest(doc.OuterXml, ruleXmlDoc.OuterXml);
                if (!CurrentViewContext.SeriesDataAfterShuffle.IsNullOrEmpty())
                {
                    rptShuffledData.DataSource = CurrentViewContext.SeriesDataAfterShuffle.GroupBy(col => col.CmpItemId)
                                .Select(col => col.First())
                                .OrderBy(col => col.ItemSeriesItemOrder);
                    rptShuffledData.DataBind();
                    divShuffledData.Visible = true;
                }
                else
                {
                    divShuffledData.Visible = false;
                }
                if (CurrentViewContext.ShotSeriesResponse.StatusCode == AppConsts.NONE)
                {
                    ShowAlertMessage(CurrentViewContext.ShotSeriesResponse.Message, MessageType.SuccessMessage, "Success");
                }
                else
                {
                    if (CurrentViewContext.ShotSeriesResponse.StatusName.ToLower() == "error")
                    {
                        ShowAlertMessage(CurrentViewContext.ShotSeriesResponse.Message, MessageType.Error, "Validation Message(s)");

                    }
                    else if (CurrentViewContext.ShotSeriesResponse.StatusName.ToLower() == "validationfailed")
                    {
                        ShowAlertMessage(CurrentViewContext.ShotSeriesResponse.Message, MessageType.Error, "UI Rule Validation Message(s)");
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

        #region Repeater Events

        protected void rptShuffledData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    Repeater rptShuffledHeader = e.Item.FindControl("rptShuffledHeader") as Repeater;
                    List<SeriesAttributeContract> lstSeriesAttributeContract = CurrentViewContext.SeriesDataAfterShuffle.GroupBy(col => col.CmpAttributeId)
                        .Select(col => col.First()).OrderBy(col => col.CmpAttributeId).ToList();
                    lstSeriesAttributeContract.Insert(AppConsts.NONE, new SeriesAttributeContract() { CmpAttributeName = "Item Name", CmpAttributeId = AppConsts.MINUS_ONE });
                    lstSeriesAttributeContract.Insert(AppConsts.ONE, new SeriesAttributeContract() { CmpAttributeName = "Item Status", CmpAttributeId = AppConsts.NONE });
                    rptShuffledHeader.DataSource = lstSeriesAttributeContract;
                    rptShuffledHeader.DataBind();
                }
                else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblItemSeriesItemID = (Label)e.Item.FindControl("lblItemSeriesItemID");
                    Int32 itemSeriesItemID = Convert.ToInt32(lblItemSeriesItemID.Text.Trim());
                    Repeater rptShuffledColumn = e.Item.FindControl("rptShuffledColumn") as Repeater;
                    rptShuffledColumn.DataSource = CurrentViewContext.SeriesDataAfterShuffle.Where(col => col.CmpItemSeriesItemId == itemSeriesItemID).GroupBy(col => col.CmpAttributeId)
                                                .Select(col => col.First())
                                                .OrderBy(col => col.CmpAttributeId);
                    rptShuffledColumn.DataBind();
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

        protected void rptShotSeriesData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    Repeater rptHeader = e.Item.FindControl("rptHeader") as Repeater;
                    List<SeriesAttributeContract> lstSeriesAttributeContract = CurrentViewContext.SeriesData.GroupBy(col => col.CmpAttributeId).Select(col => col.First()).OrderBy(col => col.CmpAttributeId).ToList();
                    lstSeriesAttributeContract.Insert(AppConsts.NONE, new SeriesAttributeContract() { CmpAttributeName = "Item Name", CmpAttributeId = AppConsts.MINUS_ONE });
                    lstSeriesAttributeContract.Insert(AppConsts.ONE, new SeriesAttributeContract() { CmpAttributeName = "Item Status", CmpAttributeId = AppConsts.NONE });
                    rptHeader.DataSource = lstSeriesAttributeContract;
                    rptHeader.DataBind();
                }
                else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    String incompleteStatusCode = ApplicantItemComplianceStatus.Incomplete.GetStringValue();
                    Int32 incompleteStatusID = CurrentViewContext.LstItemComplianceStatus.Where(col => col.Code == incompleteStatusCode).FirstOrDefault().ItemComplianceStatusID;
                    RadComboBox ddlItemStatus = (RadComboBox)e.Item.FindControl("ddlItemStatus");
                    if (!ddlItemStatus.IsNullOrEmpty())
                    {
                        ddlItemStatus.DataSource = CurrentViewContext.LstItemComplianceStatus;
                        ddlItemStatus.DataBind();
                        ddlItemStatus.SelectedValue = incompleteStatusID.ToString();
                    }
                    Repeater rptColumn = e.Item.FindControl("rptColumn") as Repeater;
                    rptColumn.DataSource = CurrentViewContext.SeriesData.GroupBy(col => col.CmpAttributeId)
                                                .Select(col => col.First())
                                                .OrderBy(col => col.CmpAttributeId);
                    rptColumn.DataBind();
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

        protected void rptColumn_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                SeriesAttributeContract seriesAttributeContract = (SeriesAttributeContract)e.Item.DataItem;
                if (!seriesAttributeContract.IsNullOrEmpty())
                {
                    HandleAttributeDisplay(e, seriesAttributeContract);
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

        #region Drop Down Events

        protected void cmbPackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Presenter.GetRuleDetails();
                ShotSeriesShuffleRuleTest.LstSeriesRuleDetails = CurrentViewContext.LstSeriesRuleDetails;
                ShotSeriesShuffleRuleTest.BindSeriesRuleDetails();
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

        #region Public Methods

        #endregion

        #region Private Methods

        private void BindSeriesDetails()
        {
            Presenter.GetSeriesDataForShuffleTest();
            rptShotSeriesData.DataSource = CurrentViewContext.SeriesData.GroupBy(col => col.CmpItemId)
                                            .Select(col => col.First())
                                            .OrderBy(col => col.ItemSeriesItemOrder);
            rptShotSeriesData.DataBind();
            cmbPackage.DataSource = CurrentViewContext.LstPackages;
            cmbPackage.DataBind();
            cmbPackage.AddFirstEmptyItem();
            ShotSeriesShuffleRuleTest.LstSeriesRuleDetails = CurrentViewContext.LstSeriesRuleDetails;
            ShotSeriesShuffleRuleTest.LstItemComplianceStatus = CurrentViewContext.LstItemComplianceStatus;
        }

        private void ShowAlertMessage(String strMessage, MessageType msgType, String headerText)
        {
            String msgClass = "info";
            switch (msgType)
            {
                case MessageType.Error:
                    msgClass = "error";
                    break;
                case MessageType.Information:
                    msgClass = "info";
                    break;
                case MessageType.SuccessMessage:
                    msgClass = "sucs";
                    break;
                default:
                    msgClass = "info";
                    break;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlertMessage"
                                                , "$page.showAlertMessage('" + strMessage.ToString() + "','" + msgClass + "','" + headerText + "');", true);
        }

        private void HandleAttributeDisplay(RepeaterItemEventArgs e, SeriesAttributeContract seriesAttributeContract)
        {
            if (seriesAttributeContract.CmpAttributeDatatypeCode.ToLower() == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dtPicker = (WclDatePicker)e.Item.FindControl("dtPicker");
                dtPicker.Visible = true;
                if (!seriesAttributeContract.IsKeyAttribute)
                {
                    dtPicker.SelectedDate = DateTime.Now;
                }
            }
            else if (seriesAttributeContract.CmpAttributeDatatypeCode.ToLower() == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtBox = (WclTextBox)e.Item.FindControl("txtBox");
                txtBox.Visible = true;
                if (!seriesAttributeContract.IsKeyAttribute)
                {
                    txtBox.Text = "1";
                }
            }
            else if (seriesAttributeContract.CmpAttributeDatatypeCode.ToLower() == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim()
              || seriesAttributeContract.CmpAttributeDatatypeCode.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower().Trim()
                || seriesAttributeContract.CmpAttributeDatatypeCode.ToLower() == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim())
            {
                WclNumericTextBox numericTextBox = (WclNumericTextBox)e.Item.FindControl("numericTextBox");
                numericTextBox.Visible = true;
                if (!seriesAttributeContract.IsKeyAttribute)
                {
                    numericTextBox.Text = "1";
                }
            }
            else if (seriesAttributeContract.CmpAttributeDatatypeCode.ToLower() == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
            {
                String _optionsCode = ComplianceAttributeDatatypes.Options.GetStringValue();

                var _cmbData = CurrentViewContext.SeriesData
                               .Where(attrOption => attrOption.CmpAttributeId == seriesAttributeContract.CmpAttributeId
                                && attrOption.CmpAttributeDatatypeCode == _optionsCode)
                                .Distinct()
                               .ToList().Select(optn => new { optn.OptionText, optn.OptionValue }).Distinct();
                WclComboBox optionCombo = (WclComboBox)e.Item.FindControl("optionCombo");
                optionCombo.Visible = true;
                foreach (var attributeOption in _cmbData)
                    optionCombo.Items.Add(new RadComboBoxItem(attributeOption.OptionText, attributeOption.OptionValue));
                optionCombo.Items.Insert(0, new RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
                optionCombo.DataBind();
                if (!seriesAttributeContract.IsKeyAttribute)
                {
                    optionCombo.SelectedIndex = AppConsts.ONE;
                }
            }
        }

        private String GetAttributeValue(RepeaterItem item, String complianceAttributeDataTypeCode)
        {
            String value = String.Empty;
            if (complianceAttributeDataTypeCode.ToLower() == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dtPicker = (WclDatePicker)item.FindControl("dtPicker");
                if (dtPicker.SelectedDate.IsNullOrEmpty())
                {
                    value = String.Empty;
                }
                else
                {
                    value = dtPicker.SelectedDate.Value.ToShortDateString();
                }

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
            return value;
        }


        #endregion


        #endregion

    }
}