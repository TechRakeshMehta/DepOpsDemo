using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using Entity.SharedDataEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class SharedCustomAttributes : BaseUserControl, ISharedCustomAttributesView
    {

        #region Variables
        private SharedCustomAttributesPresenter _presenter = new SharedCustomAttributesPresenter();
        #region Private Variables

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public SharedCustomAttributesPresenter Presenter
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

        public ISharedCustomAttributesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        List<SharedCustomAttributesContract> ISharedCustomAttributesView.lstSharedCustomAttributes
        {
            get
            {
                if (!ViewState["lstSharedCustomAttributes"].IsNullOrEmpty())
                    return (List<SharedCustomAttributesContract>)ViewState["lstSharedCustomAttributes"];
                return new List<SharedCustomAttributesContract>();
            }
            set
            {
                ViewState["lstSharedCustomAttributes"] = value;
            }
        }

        SharedCustomAttributesContract ISharedCustomAttributesView.SharedCustomAttributes
        {
            get
            {
                if (!ViewState["SharedCustomAttributes"].IsNullOrEmpty())
                    return (SharedCustomAttributesContract)ViewState["SharedCustomAttributes"];
                return new SharedCustomAttributesContract();
            }
            set
            {
                ViewState["SharedCustomAttributes"] = value;
            }
        }

        Int32 ISharedCustomAttributesView.SelectSharedCustomAttributeID
        {
            get
            {
                if (!ViewState["SelectSharedCustomAttributeID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectSharedCustomAttributeID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectSharedCustomAttributeID"] = value;
            }
        }

        Int32 ISharedCustomAttributesView.SelectSharedCustomAttributeMappingID
        {
            get
            {
                if (!ViewState["SelectSharedCustomAttributeMappingID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectSharedCustomAttributeMappingID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectSharedCustomAttributeMappingID"] = value;
            }
        }
        List<lkpCustomAttributeDataType> ISharedCustomAttributesView.lstAttributeDataType
        {
            get
            {
                if (!ViewState["lstAttributeDataType"].IsNullOrEmpty())
                    return (List<lkpCustomAttributeDataType>)(ViewState["lstAttributeDataType"]);
                return new List<lkpCustomAttributeDataType>();
            }
            set
            {
                ViewState["lstAttributeDataType"] = value;
            }
        }

        List<lkpSharedCustomAttributeUseType> ISharedCustomAttributesView.lstAttributeUseType
        {
            get
            {
                if (!ViewState["lstAttributeUseType"].IsNullOrEmpty())
                    return (List<lkpSharedCustomAttributeUseType>)(ViewState["lstAttributeUseType"]);
                return new List<lkpSharedCustomAttributeUseType>();
            }
            set
            {
                ViewState["lstAttributeUseType"] = value;
            }
        }

        List<AgencyHierarchyContract> ISharedCustomAttributesView.lstAgencyRootNodes
        {
            get
            {
                if (!ViewState["lstAgencyRootNodes"].IsNullOrEmpty())
                    return (List<AgencyHierarchyContract>)(ViewState["lstAgencyRootNodes"]);
                return new List<AgencyHierarchyContract>();
            }
            set
            {
                ViewState["lstAgencyRootNodes"] = value;
            }
        }

        Int32 ISharedCustomAttributesView.SelectedAgencyRootNodeID
        {
            get
            {
                if (!ViewState["SelectedAgencyRootNodeID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedAgencyRootNodeID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedAgencyRootNodeID"] = value;
            }
        }


        Guid ISharedCustomAttributesView.UserId
        {
            get
            {
                return base.SysXMembershipUser.UserId;
            }
        }

        public Boolean IsAgencyUserLoggedIn
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                List<String> lstSharedUserTypeCode = user.SharedUserTypesCode;
                if (lstSharedUserTypeCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()) && user.IsSharedUser)
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #endregion


        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Shared Custom Attributes";
                base.SetPageTitle("Shared Custom Attributes");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    GetAgencyHierarchyRootNodes();
                    GetAttributeDataTypes();
                    GetAttributeUseTypes();
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

        #region Grid Events

        protected void grdSharedCustomAttribbutes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    WclTextBox txtAttributeName = e.Item.FindControl("txtAttributeName") as WclTextBox;
                    WclTextBox txtAttributeLabel = e.Item.FindControl("txtAttributeLabel") as WclTextBox;
                    RadioButtonList rblIsActive = e.Item.FindControl("rblIsActive") as RadioButtonList;
                    WclComboBox cmbDataType = e.Item.FindControl("cmbDataType") as WclComboBox;
                    WclComboBox cmbUseType = e.Item.FindControl("cmbUseType") as WclComboBox;
                    RadioButtonList rblIsRequired = e.Item.FindControl("rblIsRequired") as RadioButtonList;
                    WclNumericTextBox txtTextMaxChars = e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox;
                    WclTextBox txtRegularExp = e.Item.FindControl("txtRegularExp") as WclTextBox;
                    WclTextBox txtRegExpErrorMsg = e.Item.FindControl("txtRegExpErrorMsg") as WclTextBox;
                    WclTextBox txtValString = e.Item.FindControl("txtValString") as WclTextBox;
                    WclComboBox cmbRelatedAttribute = e.Item.FindControl("cmbRelatedAttribute") as WclComboBox;
                    WclComboBox cmbAgency = e.Item.FindControl("cmbAgency") as WclComboBox;

                    CurrentViewContext.SharedCustomAttributes = new SharedCustomAttributesContract();

                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.SharedCustomAttributes.SharedCustomAttributeID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SharedCustomAttributeID"]);
                        CurrentViewContext.SharedCustomAttributes.SharedCustomAttributeMappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SharedCustomAttributeMappingID"]);
                    }

                    CurrentViewContext.SharedCustomAttributes.AttributeName = txtAttributeName.IsNullOrEmpty() ? String.Empty : (txtAttributeName.Text.IsNullOrEmpty() ? String.Empty : txtAttributeName.Text.Trim());
                    CurrentViewContext.SharedCustomAttributes.AttributeLabel = txtAttributeLabel.IsNullOrEmpty() ? String.Empty : (txtAttributeLabel.Text.IsNullOrEmpty() ? String.Empty : txtAttributeLabel.Text.Trim());
                    CurrentViewContext.SharedCustomAttributes.IsActive = rblIsActive.IsNullOrEmpty() ? false : (rblIsActive.SelectedValue.IsNullOrEmpty() ? false : Convert.ToBoolean(rblIsActive.SelectedValue));
                    CurrentViewContext.SharedCustomAttributes.AttributeDataTypeID = cmbDataType.IsNullOrEmpty() ? AppConsts.NONE : (cmbDataType.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbDataType.SelectedValue));
                    CurrentViewContext.SharedCustomAttributes.AttributeUseTypeID = cmbUseType.IsNullOrEmpty() ? AppConsts.NONE : (cmbUseType.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbUseType.SelectedValue));
                    CurrentViewContext.SharedCustomAttributes.IsRequired = rblIsRequired.IsNullOrEmpty() ? false : (rblIsRequired.SelectedValue.IsNullOrEmpty() ? false : Convert.ToBoolean(rblIsRequired.SelectedValue));

                    //if (txtTextMaxChars.IsNullOrEmpty())
                    //    CurrentViewContext.SharedCustomAttributes.StringLength = null;
                    //else
                    //    CurrentViewContext.SharedCustomAttributes.StringLength = (txtTextMaxChars.Text.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(txtTextMaxChars.Text));

                    CurrentViewContext.SharedCustomAttributes.StringLength = txtTextMaxChars.IsNullOrEmpty() ? null : (txtTextMaxChars.Text.IsNullOrEmpty() ? (Int32?)null : (Int32?)(Convert.ToInt32(txtTextMaxChars.Text)));
                    CurrentViewContext.SharedCustomAttributes.RegularExpression = txtRegularExp.IsNullOrEmpty() ? null : (txtRegularExp.Text.IsNullOrEmpty() ? null : txtRegularExp.Text.Trim());
                    CurrentViewContext.SharedCustomAttributes.RegExpErrorMsg = txtRegExpErrorMsg.IsNullOrEmpty() ? null : (txtRegExpErrorMsg.Text.IsNullOrEmpty() ? null : txtRegExpErrorMsg.Text.Trim());
                    CurrentViewContext.SharedCustomAttributes.RelatedCustomAttributeID = cmbRelatedAttribute.IsNullOrEmpty() ? null : (cmbRelatedAttribute.SelectedValue.IsNullOrEmpty() ? (Int32?)null : (Int32?)Convert.ToInt32(cmbRelatedAttribute.SelectedValue));

                    if (CurrentViewContext.IsAgencyUserLoggedIn)
                        CurrentViewContext.SharedCustomAttributes.AgencyHierarchyRootNodeID = CurrentViewContext.SelectedAgencyRootNodeID;
                    else
                        CurrentViewContext.SharedCustomAttributes.AgencyHierarchyRootNodeID = Convert.ToInt32(cmbAgency.SelectedValue);

                    if (Presenter.SaveSharedCustomAttribute())
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Attribute saved successfully.");
                        grdSharedCustomAttribbutes.Rebind();
                    }
                    else
                    {
                        e.Canceled = true;
                        base.ShowErrorMessage("Some error occurred. Please try again.");
                    }
                }

                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.SelectSharedCustomAttributeID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SharedCustomAttributeID"]);
                    CurrentViewContext.SelectSharedCustomAttributeMappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SharedCustomAttributeMappingID"]);

                    if (!CurrentViewContext.SelectSharedCustomAttributeID.IsNullOrEmpty() && CurrentViewContext.SelectSharedCustomAttributeID > AppConsts.NONE
                        && !CurrentViewContext.SelectSharedCustomAttributeMappingID.IsNullOrEmpty() && CurrentViewContext.SelectSharedCustomAttributeMappingID > AppConsts.NONE)
                    {
                        if (Presenter.DeleteSharedCustomAttribute())
                        {
                            e.Canceled = false;
                            base.ShowSuccessMessage("Attribute deleted successfully.");
                            grdSharedCustomAttribbutes.Rebind();
                        }
                        else
                        {
                            e.Canceled = true;
                            base.ShowErrorMessage("Some error occurred. Please try again.");
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

        protected void grdSharedCustomAttribbutes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetSharedCustomAttributes();
                grdSharedCustomAttribbutes.DataSource = CurrentViewContext.lstSharedCustomAttributes;
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

        protected void grdSharedCustomAttribbutes_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    WclComboBox cmbDataType = e.Item.FindControl("cmbDataType") as WclComboBox;
                    WclComboBox cmbUseType = e.Item.FindControl("cmbUseType") as WclComboBox;
                    WclComboBox cmbAgency = e.Item.FindControl("cmbAgency") as WclComboBox;
                    RequiredFieldValidator rfvAgency = e.Item.FindControl("rfvAgency") as RequiredFieldValidator;

                    if (!cmbAgency.IsNullOrEmpty())
                    {
                        cmbAgency.DataSource = CurrentViewContext.lstAgencyRootNodes.Distinct();
                        cmbAgency.DataBind();
                        cmbAgency.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));

                        if (CurrentViewContext.IsAgencyUserLoggedIn && CurrentViewContext.SelectedAgencyRootNodeID > AppConsts.NONE)
                        {
                            cmbAgency.SelectedValue = CurrentViewContext.SelectedAgencyRootNodeID.ToString();
                            cmbAgency.Enabled = false;
                            rfvAgency.Enabled = false;
                        }
                        else
                        {
                            cmbAgency.Enabled = true;
                            rfvAgency.Enabled = true;
                        }
                    }

                    if (!cmbDataType.IsNullOrEmpty())
                    {
                        cmbDataType.DataSource = CurrentViewContext.lstAttributeDataType;
                        cmbDataType.DataBind();
                        cmbDataType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
                    }

                    if (!cmbUseType.IsNullOrEmpty())
                    {
                        cmbUseType.DataSource = CurrentViewContext.lstAttributeUseType;
                        cmbUseType.DataBind();
                        cmbUseType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
                    }

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        SharedCustomAttributesContract sharedCustomAttribute = (SharedCustomAttributesContract)e.Item.DataItem;
                        if (!sharedCustomAttribute.IsNullOrEmpty())
                        {
                            RadioButtonList rblIsActive = e.Item.FindControl("rblIsActive") as RadioButtonList;
                            RadioButtonList rblIsRequired = e.Item.FindControl("rblIsRequired") as RadioButtonList;
                            HtmlGenericControl dvValidate = e.Item.FindControl("dvValidate") as HtmlGenericControl;
                            HtmlGenericControl dvTextTypeInputs = e.Item.FindControl("dvTextTypeInputs") as HtmlGenericControl;

                            rblIsActive.SelectedValue = sharedCustomAttribute.IsActive.IsNullOrEmpty() ? false.ToString() : sharedCustomAttribute.IsActive.ToString();
                            rblIsRequired.SelectedValue = sharedCustomAttribute.IsRequired.IsNullOrEmpty() ? false.ToString() : sharedCustomAttribute.IsRequired.ToString();
                            if (!cmbDataType.IsNullOrEmpty() && !sharedCustomAttribute.AttributeDataTypeID.IsNullOrEmpty() && sharedCustomAttribute.AttributeDataTypeID > AppConsts.NONE)
                                cmbDataType.SelectedValue = sharedCustomAttribute.AttributeDataTypeID.ToString();
                            if (!cmbUseType.IsNullOrEmpty() && !sharedCustomAttribute.AttributeUseTypeID.IsNullOrEmpty() && sharedCustomAttribute.AttributeUseTypeID > AppConsts.NONE)
                            {
                                cmbUseType.SelectedValue = sharedCustomAttribute.AttributeUseTypeID.ToString();
                                cmbUseType.Enabled = false;
                            }
                            if (!cmbAgency.IsNullOrEmpty() && !sharedCustomAttribute.AgencyHierarchyRootNodeID.IsNullOrEmpty() && sharedCustomAttribute.AgencyHierarchyRootNodeID > AppConsts.NONE)
                            {
                                cmbAgency.SelectedValue = sharedCustomAttribute.AgencyHierarchyRootNodeID.ToString();
                            }
                            String dataTypeCode = CurrentViewContext.lstAttributeDataType.Where(cond => cond.CustomAttributeDataTypeID == sharedCustomAttribute.AttributeDataTypeID).FirstOrDefault().Code;
                            if (!String.IsNullOrEmpty(dataTypeCode) && dataTypeCode == CustomAttributeDatatype.Text.GetStringValue())
                            {
                                //WclNumericTextBox txtTextMaxChars = e.Item.FindControl("txtTextMaxChars") as WclNumericTextBox;
                                //WclTextBox txtRegularExp = e.Item.FindControl("txtRegularExp") as WclTextBox;
                                //WclTextBox txtRegExpErrorMsg = e.Item.FindControl("txtRegExpErrorMsg") as WclTextBox;
                                //WclTextBox txtValString = e.Item.FindControl("txtValString") as WclTextBox;
                                dvTextTypeInputs.Visible = true;
                                dvValidate.Visible = true;
                            }
                            else
                            {
                                dvTextTypeInputs.Visible = false;
                                dvValidate.Visible = false;
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

        #endregion

        #region Button Events

        protected void btnValidateRegExp_Click(object sender, EventArgs e)
        {
            try
            {
                WclButton btnvalidateRegExp = sender as WclButton;
                System.Web.UI.HtmlControls.HtmlGenericControl dvValidateRegExp = (System.Web.UI.HtmlControls.HtmlGenericControl)btnvalidateRegExp.Parent;

                System.Web.UI.WebControls.Panel panel = (System.Web.UI.WebControls.Panel)dvValidateRegExp.Parent;
                System.Web.UI.HtmlControls.HtmlGenericControl dvTextTypeInputs = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("dvTextTypeInputs");
                System.Web.UI.HtmlControls.HtmlGenericControl dvValidate = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("dvValidate");

                Label lblValidStatus = dvValidateRegExp.FindControl("lblValidStatus") as Label;
                WclTextBox txtRegularExp = dvTextTypeInputs.FindControl("txtRegularExp") as WclTextBox;
                WclTextBox txtInputString = dvValidate.FindControl("txtValString") as WclTextBox;

                if (txtRegularExp.Text.IsNullOrEmpty())
                {
                    lblValidStatus.Text = "Please enter Regular Expression to Validate.";
                    lblValidStatus.ForeColor = System.Drawing.Color.Red;
                }
                else if (txtInputString.Text.IsNullOrEmpty())
                {
                    lblValidStatus.Text = "Please enter Input Text to Validate.";
                    lblValidStatus.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    if (Regex.IsMatch(txtInputString.Text, txtRegularExp.Text))
                    {
                        lblValidStatus.Text = "Success";
                        lblValidStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblValidStatus.Text = "Failed";
                        lblValidStatus.ForeColor = System.Drawing.Color.Red;
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

        #region Dropdown Events

        protected void cmbDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox cmbDataType = sender as WclComboBox;
                System.Web.UI.WebControls.Panel panel = (System.Web.UI.WebControls.Panel)cmbDataType.Parent;

                //WclComboBox cmbUseType = panel.FindControl("cmbUseType") as WclComboBox;
                if (!cmbDataType.SelectedValue.IsNullOrEmpty())
                {
                    HtmlGenericControl dvValidate = panel.FindControl("dvValidate") as HtmlGenericControl;
                    HtmlGenericControl dvTextTypeInputs = panel.FindControl("dvTextTypeInputs") as HtmlGenericControl;
                    WclNumericTextBox txtTextMaxChars = panel.FindControl("ntxtTextMaxChars") as WclNumericTextBox;
                    WclTextBox txtRegularExp = panel.FindControl("txtRegularExp") as WclTextBox;
                    WclTextBox txtRegExpErrorMsg = panel.FindControl("txtRegExpErrorMsg") as WclTextBox;

                    Int32 selectedAttributeDataTypeId = Convert.ToInt32(cmbDataType.SelectedValue);
                    String selectedAttributeDataTypeCode = CurrentViewContext.lstAttributeDataType.Where(cond => cond.CustomAttributeDataTypeID == selectedAttributeDataTypeId).FirstOrDefault().Code;
                    if (selectedAttributeDataTypeCode == CustomAttributeDatatype.Text.GetStringValue())
                    {
                        dvTextTypeInputs.Visible = true;
                        dvValidate.Visible = true;
                    }
                    else
                    {
                        dvTextTypeInputs.Visible = false;
                        dvValidate.Visible = false;
                        txtTextMaxChars.Text = String.Empty;
                        txtRegularExp.Text = String.Empty;
                        txtRegExpErrorMsg.Text = String.Empty;
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

        private void GetAttributeDataTypes()
        {
            Presenter.GetSharedAttributeDataTypes();
        }

        private void GetAttributeUseTypes()
        {
            Presenter.GetSharedAttributeUseTypes();
        }

        private void GetAgencyHierarchyRootNodes()
        {
            Presenter.GetAgencyHierarchyRootNodes();
            if (CurrentViewContext.IsAgencyUserLoggedIn)
            {
                Presenter.GetAgencyRootNode();
            }
        }

        private void GetRelatedAttributes()
        {
            //Presenter.GetRelatedAttributes();
        }

        private void ManageControlsVisibility()
        {

        }

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}